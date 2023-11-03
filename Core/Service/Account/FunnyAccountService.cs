using System;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;
using System.Threading.Tasks;
using UnityEngine.Analytics;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyAccountService : IFunnyAccountAPI
    {
        private readonly BridgeService Service;

        private readonly PrivateInfoAuthTrack PrivateInfoTrack;
        private readonly LoginTrack LoginAnalysis;

        private UserProfile CurrentUserProfile => AccountInfo.Current.Profile;
        private BindInfo CurrentBindInfo => AccountInfo.Current.BindInfo;

        public event Action OnLogoutEvents;
        public event Action<AccessToken> OnLoginEvents;
        public event Action<AccessToken> OnSwitchAccountEvents;

        internal FunnyAccountService(BridgeService bridgeService)
        {
            Service = bridgeService;
            PrivateInfoTrack = new PrivateInfoAuthTrack(bridgeService.Analysis);
            LoginAnalysis = new LoginTrack(bridgeService.Analysis);

            BridgeNotificationCenter.Default.AddObserver(this, "event.logout", () =>
            {
                AccountInfo.Current.Clear();
                OnLogoutEvents?.Invoke();
            });

            BridgeNotificationCenter.Default.AddObserver(this, "event.switch.account", (value) =>
            {
                if (value.TryGet<AccessToken>(out var accessToken))
                {
                    AccountInfo.Current.Profile = GetUserProfile();
                    OnSwitchAccountEvents?.Invoke(accessToken);
                }
                else
                {
                    Logger.LogError($"Event value error - event.switch.account - {value.RawValue}");
                }
            });

            BridgeNotificationCenter.Default.AddObserver(this, "event.bind.status.change", async () =>
            {
                try
                {
                    AccountInfo.Current.Profile = Service.Login.GetUserProfile();
                    AccountInfo.Current.BindInfo = await Service.Bind.FetchBindInfo().Async();
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Event value error - event.bind.status.change - {ex.Message}");
                }
            });

        }

        public void AuthPrivateUserInfo(Action<UserPrivateInfo> onSuccessAction, Action<bool> onCancelAction)
        {
            PrivateInfoTrack.Start();

            if (CurrentUserProfile is null)
            {
                PrivateInfoTrack.FailureResult(ServiceError.Make(ServiceErrorType.NoLoginError));
                onCancelAction?.Invoke(false);
                return;
            }

            if (CurrentUserProfile.PrivateInfo is null) // 开关判断
            {
                PrivateInfoTrack.NotEnabled();
                onCancelAction?.Invoke(false);
            }
            else if (CurrentUserProfile.PrivateInfo.Filled) // 信息完整判断
            {
                PrivateInfoTrack.SuccessResult();
                onSuccessAction?.Invoke(CurrentUserProfile.PrivateInfo);
            }
            else
            {
                // 打开 UI 填写
                FetchPrivateInfoHandler(CurrentUserProfile.PrivateInfo, onSuccessAction, onCancelAction);
            }

        }

        public void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate)
        {
            Logger.Log("发起用户信息授权 - GetPrivateUserInfo");

            AuthPrivateUserInfo((info) =>
            {
                serviceDelegate?.OnConsentAuthPrivateInfo(info);

            }, (enableService) =>
            {
                if (enableService)
                {
                    serviceDelegate?.OnNextTime();
                }
                else
                {
                    serviceDelegate?.OnUnenabledService();
                }
            });
        }

        private void FetchPrivateInfoHandler(UserPrivateInfo info, Action<UserPrivateInfo> onSuccessAction, Action<bool> onCancelAction)
        {

            AdditionalInfoView.OnNextTimeAction = () =>
            {
                PrivateInfoTrack.Cancel();
                AdditionalInfoView.Close();

                onCancelAction?.Invoke(true);
            };

            AdditionalInfoView.OnCommitAction = async (sexValue, birthday) =>
            {
                Loader.ShowIndicator();
                try
                {
                    await Service.Login.CommitPrivateInfo(birthday, sexValue).Async();
                    AccountInfo.Current.Profile = await Service.Login.FetchUserProfile().Async();

                    PrivateInfoTrack.SuccessResult();

                    Loader.HideIndicator();
                    AdditionalInfoView.Close();
                    onSuccessAction?.Invoke(CurrentUserProfile.PrivateInfo);
                }
                catch (ServiceError error)
                {
                    PrivateInfoTrack.FailureResult(error);

                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }

            };

            AdditionalInfoView.Open(info.Gender, info.Birthday);
        }

        public void GetUserProfile(IUserServiceDelegate serviceDelegate)
        {
            Service.Login.FetchUserProfile()
                         .Then((userProfile) =>
                         {
                             AccountInfo.Current.Profile = userProfile;
                             serviceDelegate?.OnUserProfileSuccess(userProfile);
                         })
                         .Catch((error) =>
                         {
                             serviceDelegate?.OnUserProfileFailure((ServiceError)error);
                         });
        }

        public void FetchUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler)
        {
            Service.Login.FetchUserProfile()
                         .Then((profile) =>
                         {
                             AccountInfo.Current.Profile = profile;
                             onSuccessHandler(profile);
                         })
                         .Catch((error) =>
                         {
                             onFailureHandler((ServiceError)error);
                         });
        }

        public void GetUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler)
        {
            FetchUserProfile(onSuccessHandler, onFailureHandler);
        }

        public void Login(ILoginServiceDelegate serviceDelegate)
        {
            Login((token) =>
            {
                serviceDelegate?.OnLoginSuccess(token);
            }, (error) =>
            {
                serviceDelegate?.OnLoginFailure(error);
            }, () =>
            {
                serviceDelegate?.OnLoginCancel();
            });
        }

        private bool StartFlag = false;

        public async void Login(Action<AccessToken> onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler)
        {

            if (StartFlag)
            {
                Logger.LogWarning("已有登录正在进行中，请等待完成。");
                return;
            }

            try
            {
                StartFlag = true;

                LoginAnalysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

                Loader.ShowIndicator();

                AppInfoConfig appInfo = await Service.Common.GetAppInfo().Async();

                LoginView.SetProviders(appInfo.GetLoginProviders());
                LoginUIFlowSetup(onSuccessHandler, onCancelHandler);

                if (Service.Login.IsAuthorized) // 快速登录
                {
                    LimitStatus limitStatus = await VerifyLimit();
                    Loader.HideIndicator();

                    LoginAnalysis.SdkStartLoginSuccess(true, true);

                    LimitResultHandler(limitStatus, false, onSuccessHandler);
                }
                else if (appInfo.EnableAutoGuest) // 无感登录
                {

                    LoginResult loginResult = await Service.Login.LoginWithProvider(LoginProvider.Guest).Async();

                    // TODO: 待定无感登录埋点
                    //LoginAnalysis.SdkStartLoginSuccess(true, true);

                    if (loginResult.IsNeedBind) // 需进行手机号绑定
                    {
                        Toast.ShowFail("需要进行绑定流程");
                        Loader.HideIndicator();
                    }
                    else
                    {
                        // 登录限制效验
                        LimitStatus limitStatus = await VerifyLimit();

                        Loader.HideIndicator();

                        LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                    }
                }
                else // 打开登录 UI 界面流程
                {
                    Loader.HideIndicator();
                    LoginView.Open();
                }
            }
            catch (ServiceError error)
            {
                Loader.HideIndicator();

                if (error.Error == ServiceErrorType.InvalidAccessToken)
                {
                    Toast.ShowFail(error.Message);

                    LoginAnalysis.SdkLoginResultFailure(false, error);
                    LoginAnalysis.SdkStartLoginFailure(true, true, error);

                    LoginAnalysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

                    LoginView.Open();
                }
                else
                {
                    LoginAnalysis.SdkStartLoginFailure(true, true, error);

                    StartFlag = false;
                    onFailureHandler?.Invoke(error);
                }
            }
        }

        internal async Task<LimitStatus> VerifyLimit()
        {
            LimitStatus status = await Service.Login.NativeVerifyLimit().Async();

            AccountInfo.Current.Profile = await Service.Login.FetchUserProfile().Async();
            AccountInfo.Current.BindInfo = await Service.Bind.FetchBindInfo().Async();
            AccountInfo.Current.Token = Service.Login.GetCurrentAccessToken();

            return status;
        }

        private void LimitResultHandler(LimitStatus limit, bool newUser, Action<AccessToken> onSuccessHandler)
        {
            switch (limit.Status)
            {
                case LimitStatus.StatusType.Success:
                    {
                        StartFlag = false;

                        // 关闭 UI 以及后续逻辑
                        LoginView.Close();
                        AccessToken token = GetCurrentAccessToken();

                        LoginAnalysis.SdkLoginResultSuccess(newUser);

                        onSuccessHandler?.Invoke(token);
                        OnLoginEvents?.Invoke(token);
                    }
                    break;
                case LimitStatus.StatusType.AccountBannedFailed:
                    {
                        Loader.ShowIndicator();
                        WebPCInfo info = new WebPCInfo();

                        Service.Login.GetWebPCInfo()
                                     .Then((pcInfo) =>
                                     {
                                         info = pcInfo;
                                     })
                                     .Catch((error) =>
                                     {
                                         Toast.ShowFail(error.Message);
                                         Logger.LogError(error.Message);
                                     })
                                     .Finally(() =>
                                     {
                                         Loader.HideIndicator();
                                         string tips = string.Format(Locale.LoadText("page.block.hours"), info.BanDate);

                                         if (info.UnblockedAt == -1)
                                         {
                                             tips = Locale.LoadText("page.block.forever");//"您的账号已永久封停，如有疑问，请联系客服";
                                         }
                                         // 账号已被封禁处理
                                         LoginView.JumpTo(UILoginPageState.LoginLimitPage, tips);
                                     });
                    }

                    break;
                case LimitStatus.StatusType.AllowFailed:
                    // IP 限制页面
                    LoginView.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
                case LimitStatus.StatusType.AccountInCooldownFailed:
                    {
                        Loader.ShowIndicator();
                        WebPCInfo info = new WebPCInfo();

                        Service.Login.GetWebPCInfo()
                                     .Then((pcInfo) =>
                                     {
                                         info = pcInfo;
                                     })
                                     .Catch((error) =>
                                     {
                                         Logger.LogError(error.Message);
                                         Toast.ShowFail(error.Message);
                                     })
                                     .Finally(() =>
                                     {
                                         Loader.HideIndicator();
                                         //string tips = $"账号 {info.Account} 于 {info.StartDate} 提交了永久删除账号申请，将于 {info.DeadlineDate} 永久删除。如需要保留账号，请点击下方按钮撤回申请";
                                         string tips = string.Format(Locale.LoadText("message.account.delete.tipsAfterLogin"), info.Account, info.StartDate, info.DeadlineDate);
                                         // 账号冷静期页面
                                         LoginView.JumpTo(UILoginPageState.CoolDownTipsPage, tips);
                                     });
                    }
                    break;
                case LimitStatus.StatusType.ActivationFailed:
                    Toast.ShowFail(Locale.LoadText("page.activeCode.invalid"));

                    LoginAnalysis.SdkVerifyCodeFailure(2, 402, new ServiceError(limit.StatusCode, "无效邀请码"));
                    LoginView.JumpTo(UILoginPageState.ActivationKeyPage);
                    break;
                case LimitStatus.StatusType.ActivationUnfilled:
                    // 跳转邀请码页面
                    LoginView.JumpTo(UILoginPageState.ActivationKeyPage);
                    break;
                default:
                    Toast.ShowFail(Locale.LoadText("message.error.unknown"));
                    LoginView.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
            }
        }

        private void LoginUIFlowSetup(Action<AccessToken> onSuccessHandler, Action onCancelHandler)
        {
            LoginView.OnCancelAction = (pageState) =>
            {
                StartFlag = false;

                LoginAnalysis.SdkPageClose((int)pageState);
                LoginAnalysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录被取消"));

                onCancelHandler?.Invoke();
            };

            LoginView.OnOpenViewAction = (current, prev) =>
            {
                LoginAnalysis.SdkPageLoad((int)current, (int)prev);
            };

            LoginView.OnLoginWithPasswordAction = async (account, password) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(1);
                LoginAnalysis.SetLoginWay(BridgeConfig.IsMainland ? 103 : 101);

                try
                {
                    LoginResult loginResult = await Service.Login.LoginWithPassword(account, password).Async();
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

                    FunnyDataStore.AddAccountHistory(account);

                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();
                    LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);

                    LoginAnalysis.SdkStartLoginFailure(false, true, error);
                }
            };

            LoginView.OnLoginWithCodeAction = async (account, code) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(3);
                LoginAnalysis.SetLoginWay(BridgeConfig.IsMainland ? 103 : 101);

                try
                {
                    LoginResult loginResult = await Service.Login.LoginWithCode(account, code).Async();

                    LoginAnalysis.SdkVerifyCodeSuccess(1, 202);
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

                    FunnyDataStore.AddAccountHistory(account);

                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();

                    LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);

                    LoginAnalysis.SdkVerifyCodeFailure(1, 202, error);
                    LoginAnalysis.SdkStartLoginFailure(false, true, error);
                }

            };

            LoginView.OnLoginWithProviderAction = async (provider) =>
            {
                Loader.ShowIndicator();

                if (provider == LoginProvider.Guest)
                {
                    LoginAnalysis.SetLoginFrom(5);
                }
                else
                {
                    LoginAnalysis.SetLoginFrom(4);
                }

                LoginAnalysis.SetLoginWay((int)provider);

                try
                {
                    LoginResult loginResult = await Service.Login.LoginWithProvider(provider).Async();

                    LoginAnalysis.SdkStartLoginSuccess(false, true);

                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();

                    LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);

                    if (error.Code == 0)
                    {
                        LoginAnalysis.SdkTPAuthCancel();
                    }
                    else if (error.Code == -1)
                    {
                        LoginAnalysis.SdkTPAuthFailure(error);
                    }
                    else
                    {
                        LoginAnalysis.SdkStartLoginFailure(false, true, error);
                    }
                }
            };

            LoginView.OnRegisterAccountAction = async (account, password, code) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(2);

                try
                {
                    LoginResult loginResult = await Service.Login.RegisterAccount(account, password, code).Async();

                    LoginAnalysis.SdkVerifyCodeSuccess(1, 203);
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

                    FunnyDataStore.AddAccountHistory(account);

                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();

                    LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);

                    LoginAnalysis.SdkVerifyCodeFailure(1, 203, error);
                    LoginAnalysis.SdkStartLoginFailure(false, true, error);
                }

            };

            LoginView.OnRetrievePasswordAction = async (account, newPassword, code) =>
            {
                Loader.ShowIndicator();
                try
                {
                    await Service.Login.RetrievePassword(account, newPassword, code).Async();

                    Loader.HideIndicator();
                    Toast.ShowSuccess(Locale.LoadText("message.password.change.success"));
                    LoginAnalysis.SdkVerifyCodeSuccess(1, 301);

                    LoginView.JumpTo(UILoginPageState.PwdLoginPage);
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                    LoginAnalysis.SdkVerifyCodeFailure(1, 301, error);
                }
            };

            LoginView.OnClickPriacyProtocol = () =>
            {
                Service.Common.OpenPrivacyProtocol();
            };

            LoginView.OnClickUserAgreenment = () =>
            {
                Service.Common.OpenUserAgreenment();
            };

            LoginView.OnClickContactUS = () =>
            {
                LoginAnalysis.SdkPageOpen(702);
                Service.Common.ContactUS();
            };

            LoginView.OnSwitchOtherAction = () =>
            {
                LoginAnalysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录账号被限制"));

                Logout();

                LoginAnalysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

                LoginView.JumpTo(UILoginPageState.LoginSelectPage);
            };

            LoginView.OnCommitActivationAction = (code) =>
            {
                Loader.ShowIndicator();
                Service.Login.ActivationCodeCommit(code)
                             .Then((limitStatus) =>
                             {
                                 Loader.HideIndicator();
                                 LimitResultHandler(limitStatus, false, onSuccessHandler);
                             })
                             .Catch((error) =>
                             {
                                 Loader.HideIndicator();
                                 Toast.ShowFail(error.Message);

                                 LoginAnalysis.SdkVerifyCodeFailure(2, 402, (ServiceError)error);
                             });
            };

            LoginView.OnReCallDeleteAction = () =>
            {
                string title = Locale.LoadText("alert.title.tips");
                string content = Locale.LoadText("alert.account.delete.content");
                string ok = Locale.LoadText("form.button.confirmNoSpace");
                string cancel = Locale.LoadText("form.button.cancel");

                Alert.Show(title, content,
                    new AlertActionItem(cancel),
                    new AlertActionItem(ok, async () =>
                    {
                        Loader.ShowIndicator();
                        try
                        {
                            await Service.Login.RecallAccountDelete().Async();
                            LimitStatus limitStatus = await VerifyLimit();

                            Loader.HideIndicator();

                            LimitResultHandler(limitStatus, false, onSuccessHandler);
                        }
                        catch (Exception error)
                        {
                            Loader.HideIndicator();
                            Toast.ShowFail(error.Message);
                        }
                    }));
            };

            LoginView.OnSendVerifcationCodeAction = (page, error) =>
            {
                if (error is null)
                {
                    LoginAnalysis.SdkSendCodeSuccess((int)page);
                }
                else
                {
                    LoginAnalysis.SdkSendCodeFailure((int)page, error);
                }
            };

        }


        public void Logout()
        {
            AccountInfo.Current.Clear();

            Service.Login.Logout();
            OnLogoutEvents?.Invoke();
        }

        public AccessToken GetCurrentAccessToken()
        {
            return Service.Login.GetCurrentAccessToken();
        }

        public UserProfile GetUserProfile()
        {
            return Service.Login.GetUserProfile();
        }

        public void Bind(BindingType type, Action onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler)
        {
            if (!Service.Login.IsAuthorized) // 是否已授权登录
            {
                onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            if (CurrentBindInfo.Bounded(type)) // 当前类似是否已绑定
            {
                onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.AccountBindFailed));
                return;
            }

            IBindable bindable = null;

            switch (type)
            {
                case BindingType.Email:
                    // TODO: 后续需优化调整逻辑代码
                    BindView.OnCancelAction = () =>
                    {
                        onCancelHandler?.Invoke();
                    };

                    BindView.OnCommitAction = (email, pwd, code) =>
                    {
                        Loader.ShowIndicator();
                        bindable = new EmailBindable(email, pwd, code);

                        Service.Bind.Binding(bindable, (_, error) =>
                        {
                            if (error is null)
                            {
                                Service.Bind.FetchBindInfo((info, infoError) =>
                                {

                                    AccountInfo.Current.Profile = GetUserProfile();
                                    AccountInfo.Current.BindInfo = info;

                                    Loader.HideIndicator();
                                    BindView.Close();

                                    if (infoError is null)
                                    {
                                        onSuccessHandler?.Invoke();
                                    }
                                    else
                                    {
                                        onFailureHandler?.Invoke(infoError);
                                    }
                                });
                            }
                            else
                            {
                                Loader.HideIndicator();
                                Toast.ShowFail(error.Message);
                            }
                        });
                    };

                    BindView.Open();

                    return;
                case BindingType.AppleID:
                    bindable = new AppleIdBindable();
                    break;
                case BindingType.Google:
                    bindable = new GoogleBindable();
                    break;
                default:
                    onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.UnknownError));
                    return;
            }

            Service.Bind.Binding(bindable, (_, error) =>
            {
                if (error is null)
                {

                    Service.Bind.FetchBindInfo((info, infoError) =>
                    {
                        AccountInfo.Current.Profile = GetUserProfile();
                        AccountInfo.Current.BindInfo = info;

                        if (infoError is null)
                        {
                            onSuccessHandler?.Invoke();
                        }
                        else
                        {
                            onFailureHandler?.Invoke(infoError);
                        }
                    });
                }
                else if (error.Code == 0)
                {
                    onCancelHandler?.Invoke();
                }
                else
                {
                    onFailureHandler?.Invoke(error);
                }
            });
        }

        public BindStatusItem[] GetBindStatus()
        {
            if (CurrentBindInfo is null)
            {
                return new BindStatusItem[] { };
            }

            return CurrentBindInfo.Items.ToArray();
        }
    }

}

