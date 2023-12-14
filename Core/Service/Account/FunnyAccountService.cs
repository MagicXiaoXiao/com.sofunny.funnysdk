using System;
using System.Linq;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;
using System.Threading.Tasks;


namespace SoFunny.FunnySDK
{
    internal partial class FunnyAccountService : IFunnyAccountAPI
    {
        private readonly BridgeService Service;

        private readonly PrivateInfoAuthTrack PrivateInfoTrack;
        private readonly LoginTrack LoginAnalysis;

        private UserProfile CurrentUserProfile => AccountInfo.Current.Profile;
        private BindInfo CurrentBindInfo => AccountInfo.Current.BindInfo;
        private bool _allowAutoLogin = true;

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

            BridgeNotificationCenter.Default.AddObserver(this, "event.switch.new.account", () =>
            {
                // 切换其他账号
                Logout();
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
#if !UNITY_ANDROID || UNITY_EDITOR
                Loader.ShowIndicator();
#endif
                AppInfoConfig appInfo = await Service.Common.GetAppInfo().Async();

#if UNITY_STANDALONE
                // PC 登录页设置
                PCLoginView.SetProviders(appInfo.GetLoginProviders().ToArray());
                PCLoginUIFlowSetup(onSuccessHandler, onCancelHandler);
#else
                LoginView.SetProviders(appInfo.GetLoginProviders());
                LoginUIFlowSetup(onSuccessHandler, onCancelHandler);
#endif

                if (Service.Login.IsAuthorized) // 快速登录
                {
                    LimitStatus limitStatus = await VerifyLimit();
                    Loader.HideIndicator();

                    LoginAnalysis.SdkStartLoginSuccess(true, true);

                    LimitResultHandler(limitStatus, false, onSuccessHandler);
                }
                else if (appInfo.EnableAutoGuest && _allowAutoLogin) // 无感登录
                {
                    LoginResult loginResult = await Service.Login.LoginWithProvider(LoginProvider.Guest).Async();

                    // TODO: 待定无感登录埋点
                    //LoginAnalysis.SdkStartLoginSuccess(true, true);

                    // 登录限制效验
                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();

                    LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                }
                else // 打开登录 UI 界面流程
                {
                    Loader.HideIndicator();
#if UNITY_STANDALONE
                    PCLoginView.Open();
#else
                    LoginView.Open();
#endif
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

#if UNITY_STANDALONE
                    PCLoginView.Open();
#else
                    LoginView.Open();
#endif
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
                        PCLoginView.Close();
                        BindView.Close();

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
#if UNITY_STANDALONE
                    PCLoginView.Open(PCLoginPage.AccountLimit("您已被限制登录"));
#else
                    LoginView.JumpTo(UILoginPageState.LoginLimitPage);
#endif
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
#if UNITY_STANDALONE
                                         PCLoginView.Open(PCLoginPage.AccountCooldown(tips));
#else
                                         LoginView.JumpTo(UILoginPageState.CoolDownTipsPage, tips);
#endif
                                     });
                    }
                    break;
                case LimitStatus.StatusType.ActivationFailed:
                    Toast.ShowFail(Locale.LoadText("page.activeCode.invalid"));

                    LoginAnalysis.SdkVerifyCodeFailure(2, 402, new ServiceError(limit.StatusCode, "无效邀请码"));
#if UNITY_STANDALONE
                    PCLoginView.Open(PCLoginPage.ActCode());
#else
                    LoginView.JumpTo(UILoginPageState.ActivationKeyPage);
#endif
                    break;
                case LimitStatus.StatusType.ActivationUnfilled:
                    // 跳转邀请码页面
#if UNITY_STANDALONE
                    PCLoginView.Open(PCLoginPage.ActCode());
#else
                    LoginView.JumpTo(UILoginPageState.ActivationKeyPage);
#endif
                    break;
                default:
                    Toast.ShowFail(Locale.LoadText("message.error.unknown"));
#if UNITY_STANDALONE
                    PCLoginView.Open(PCLoginPage.AccountLimit(Locale.LoadText("message.error.unknown")));
#else
                    LoginView.JumpTo(UILoginPageState.LoginLimitPage);
#endif
                    break;
            }
        }

        #region 移动端登录页逻辑处理
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

                    if (loginResult.IsNeedBind)
                    {
                        BindView.OnCancelAction = () =>
                        {
                            StartFlag = false;
                            LoginView.Close();
                            onCancelHandler?.Invoke();
                        };

                        BindView.OnCommitAction = async (account, bindCode, code) =>
                        {
                            Loader.ShowIndicator();

                            try
                            {
                                LoginResult bindResult = await Service.Bind.ForedBind(new PhoneBindable(account, code), bindCode).Async();
                                LimitStatus limitStatus = await VerifyLimit();
                                Loader.HideIndicator();

                                LimitResultHandler(limitStatus, bindResult.NewUser, onSuccessHandler);
                            }
                            catch (Exception ex)
                            {
                                Loader.HideIndicator();
                                Toast.ShowFail(ex.Message);
                            }
                        };

                        BindView.Open(loginResult.BindCode);
                        Loader.HideIndicator();
                    }
                    else
                    {
                        LoginAnalysis.SdkStartLoginSuccess(false, true);

                        LimitStatus limitStatus = await VerifyLimit();

                        Loader.HideIndicator();

                        LimitResultHandler(limitStatus, loginResult.NewUser, onSuccessHandler);
                    }
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
        #endregion

        #region PC 端登录页逻辑处理
        private void PCLoginUIFlowSetup(Action<AccessToken> onSuccessHandler, Action onCancelHandler)
        {
            PCLoginView.OnCancelAction = (pageState) =>
            {
                StartFlag = false;

                LoginAnalysis.SdkPageClose((int)pageState);
                LoginAnalysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录被取消"));

                onCancelHandler?.Invoke();
            };

            //LoginView.OnOpenViewAction = (current, prev) =>
            //{
            //    LoginAnalysis.SdkPageLoad((int)current, (int)prev);
            //};

            PCLoginView.OnLoginWithPasswordAction = async (account, password, remember) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(1);
                LoginAnalysis.SetLoginWay(BridgeConfig.IsMainland ? 103 : 101);

                try
                {
                    LoginResult loginResult = await Service.Login.LoginWithPassword(account, password).Async();
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

                    LoginAccountRecord record = FunnyDataStore.GetAccountRecord(account);

                    if (record != null)
                    {
                        if (remember)
                        {
                            record.SymbolCount = password.Length;
                        }
                        else
                        {
                            record.SymbolCount = 0;
                        }

                        FunnyDataStore.AddAccountRcord(record);
                    }

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

            PCLoginView.OnLoginWithCodeAction = async (account, code) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(3);
                LoginAnalysis.SetLoginWay(BridgeConfig.IsMainland ? 103 : 101);

                try
                {
                    LoginResult loginResult = await Service.Login.LoginWithCode(account, code).Async();

                    LoginAnalysis.SdkVerifyCodeSuccess(1, 202);
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

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

            PCLoginView.OnLoginWithRecordAction = async (record, remember) =>
            {
                Loader.ShowIndicator();

                FunnyDataStore.UpdateToken(record.Token);

                try
                {
                    if (!remember)
                    {
                        record.SymbolCount = 0;
                        FunnyDataStore.AddAccountRcord(record);
                    }

                    LimitStatus limitStatus = await VerifyLimit();

                    Loader.HideIndicator();

                    LimitResultHandler(limitStatus, false, onSuccessHandler);
                }
                catch (ServiceError error)
                {
                    if (error.Error == ServiceErrorType.InvalidAccessToken)
                    {
                        PCLoginView.OnInvaidTokenResultAction?.Invoke();
                    }

                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }
            };

            PCLoginView.OnLoginWithProviderAction = async (provider) =>
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

            PCLoginView.OnRegisterAccountAction = async (account, password, code) =>
            {
                Loader.ShowIndicator();

                LoginAnalysis.SetLoginFrom(2);

                try
                {
                    LoginResult loginResult = await Service.Login.RegisterAccount(account, password, code).Async();

                    LoginAnalysis.SdkVerifyCodeSuccess(1, 203);
                    LoginAnalysis.SdkStartLoginSuccess(false, true);

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

            PCLoginView.OnRetrievePasswordAction = async (account, newPassword, code) =>
            {
                Loader.ShowIndicator();
                try
                {
                    await Service.Login.RetrievePassword(account, newPassword, code).Async();

                    Loader.HideIndicator();
                    Toast.ShowSuccess(Locale.LoadText("message.password.change.success"));
                    LoginAnalysis.SdkVerifyCodeSuccess(1, 301);

                    PCLoginView.Open(PCLoginPage.LoginWithPassword());
                }
                catch (ServiceError error)
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                    LoginAnalysis.SdkVerifyCodeFailure(1, 301, error);
                }
            };

            PCLoginView.OnClickPriacyProtocol = () =>
            {
                Service.Common.OpenPrivacyProtocol();
            };

            PCLoginView.OnClickUserAgreenment = () =>
            {
                Service.Common.OpenUserAgreenment();
            };

            PCLoginView.OnClickContactUS = () =>
            {
                LoginAnalysis.SdkPageOpen(702);
                Service.Common.ContactUS();
            };

            PCLoginView.OnSwitchOtherAction = () =>
            {
                LoginAnalysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录账号被限制"));

                Logout();

                LoginAnalysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

                PCLoginView.Open(PCLoginPage.LoginWithPassword());
            };

            PCLoginView.OnCommitActivationAction = (code) =>
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

            PCLoginView.OnReCallDeleteAction = () =>
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

            PCLoginView.OnSendVerifcationCodeAction = (page, error) =>
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
        #endregion

        public void Logout()
        {
            if (Service.Login.IsAuthorized)
            {
                _allowAutoLogin = false;
            }

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

        public async void Bind(BindingType type, Action onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler)
        {
            if (!Service.Login.IsAuthorized) // 是否已授权登录
            {
                onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            if (CurrentBindInfo.Bounded(type)) // 当前类型是否已绑定
            {
                onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.AccountAlreadyBound));
                return;
            }

            IBindable bindable = null;

            switch (type)
            {
                case BindingType.Phone: // Tips: 不存在手机号未绑定时的已登录账号
                    onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.AccountAlreadyBound));
                    return;
                case BindingType.Email:
                    // TODO: 后续需优化调整逻辑代码
                    BindView.OnCancelAction = () =>
                    {
                        onCancelHandler?.Invoke();
                    };

                    BindView.OnCommitAction = async (email, pwd, code) =>
                    {
                        Loader.ShowIndicator();
                        bindable = new EmailBindable(email, pwd, code);

                        try
                        {
                            await Service.Bind.Binding(new EmailBindable(email, pwd, code)).Async();

                            Service.Bind.FetchBindInfo()
                                        .Then((info) =>
                                        {
                                            AccountInfo.Current.Profile = GetUserProfile();
                                            AccountInfo.Current.BindInfo = info;
                                            onSuccessHandler?.Invoke();
                                        })
                                        .Catch((error) =>
                                        {
                                            onFailureHandler?.Invoke((ServiceError)error);
                                        })
                                        .Finally(() =>
                                        {
                                            Loader.HideIndicator();
                                            BindView.Close();
                                        });
                        }
                        catch (ServiceError error)
                        {
                            Toast.ShowFail(error.Message);
                        }
                        finally
                        {
                            Loader.HideIndicator();
                        }
                    };

                    BindView.Open();

                    return;
                case BindingType.AppleID:
                    bindable = new AppleIdBindable();
                    break;
                case BindingType.Google:
                    bindable = new GoogleBindable();
                    break;
                case BindingType.QQ:
                    bindable = new QQBindable();
                    break;
                case BindingType.WeChat:
                    bindable = new WeChatBindable();
                    break;
                default:
                    onFailureHandler?.Invoke(ServiceError.Make(ServiceErrorType.AccountBindFailed));
                    return;
            }

            try
            {
                await Service.Bind.Binding(bindable).Async();

                Service.Bind.FetchBindInfo()
                            .Then((info) =>
                            {
                                AccountInfo.Current.Profile = GetUserProfile();
                                AccountInfo.Current.BindInfo = info;
                                onSuccessHandler?.Invoke();
                            })
                            .Catch((error) =>
                            {
                                onFailureHandler?.Invoke((ServiceError)error);
                            });
            }
            catch (ServiceError error)
            {
                if (error.Code == 0)
                {
                    onCancelHandler?.Invoke();
                }
                else
                {
                    onFailureHandler?.Invoke(error);
                }
            }
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

