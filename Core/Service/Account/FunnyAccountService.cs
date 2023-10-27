using System;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyAccountService : IFunnyAccountAPI, IAdditionalInfoDelegate
    {
        private readonly BridgeService Service;
        private readonly FunnyLoginService LoginService;

        private IPrivateUserInfoDelegate UserInfoDelegate;
        private readonly PrivateInfoAuthTrack PrivateInfoTrack;

        private UserProfile CurrentUserProfile;
        private BindInfo CurrentBindInfo;

        public event Action OnLogoutEvents;
        public event Action<AccessToken> OnLoginEvents;
        public event Action<AccessToken> OnSwitchAccountEvents;

        internal FunnyAccountService(FunnyLoginService loginService, BridgeService bridgeService)
        {
            Service = bridgeService;
            LoginService = loginService;
            PrivateInfoTrack = new PrivateInfoAuthTrack(bridgeService.Analysis);

            BridgeNotificationCenter.Default.AddObserver(this, "event.logout", () =>
            {
                CurrentUserProfile = null;
                OnLogoutEvents?.Invoke();
            });

            BridgeNotificationCenter.Default.AddObserver(this, "event.switch.account", (value) =>
            {
                if (value.TryGet<AccessToken>(out var accessToken))
                {
                    CurrentUserProfile = GetUserProfile();
                    OnSwitchAccountEvents?.Invoke(accessToken);
                }
                else
                {
                    Logger.LogError($"Event value error - event.switch.account - {value.RawValue}");
                }
            });
        }

        // FIXME Android 移动端待处理: Google 账号要从 Google People Api 获取用户年龄性别信息直接返回
        public void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate)
        {
            Logger.Log("发起用户信息授权 - GetPrivateUserInfo");

            UserInfoDelegate = serviceDelegate;
            PrivateInfoTrack.Start();

            if (CurrentUserProfile is null)
            {
                UserInfoDelegate?.OnPrivateInfoFailure(ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                return;
            }

            if (CurrentUserProfile.PrivateInfo is null) // 开关判断
            {
                PrivateInfoTrack.NotEnabled();

                UserInfoDelegate?.OnUnenabledService();
                UserInfoDelegate = null;
            }
            else if (CurrentUserProfile.PrivateInfo.Filled) // 信息完整判断
            {
                PrivateInfoTrack.SuccessResult();

                UserInfoDelegate?.OnConsentAuthPrivateInfo(CurrentUserProfile.PrivateInfo);
                UserInfoDelegate = null;
            }
            else
            {
                FetchPrivateInfoHandler();
            }
        }

        private void FetchPrivateInfoHandler()
        {
            Loader.ShowIndicator();

            Service.Login.GetPrivateProfile((info, error) =>
            {
                Loader.HideIndicator();

                if (error == null)
                {
                    UIService.AdditionalInfo.Open(this, info.Gender, info.Birthday);
                }
                else
                {
                    PrivateInfoTrack.FailureResult(error);

                    Logger.LogVerbose($"获取账号隐私信息失败: {error.Code} - {error.Message}");
                    UIService.AdditionalInfo.Open(this);
                }
            });
        }

        public void GetUserProfile(IUserServiceDelegate serviceDelegate)
        {
            Service.Login.FetchUserProfile((userProfile, error) =>
            {
                if (error == null)
                {
                    CurrentUserProfile = userProfile;
                    serviceDelegate?.OnUserProfileSuccess(userProfile);
                }
                else
                {
                    serviceDelegate?.OnUserProfileFailure(error);
                }
            });
        }

        public void FetchUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler)
        {

            Service.Login.FetchUserProfile((userProfile, error) =>
            {
                if (error == null)
                {
                    CurrentUserProfile = userProfile;
                    onSuccessHandler?.Invoke(userProfile);
                }
                else
                {
                    onFailureHandler?.Invoke(error);
                }
            });
        }

        public void GetUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler)
        {
            FetchUserProfile(onSuccessHandler, onFailureHandler);
        }

        public void Login(ILoginServiceDelegate serviceDelegate)
        {
            LoginService.StartLogin((token, error) =>
            {
                if (error is null)
                {
                    Service.Bind.FetchBindInfo((bindInfo, infoError) =>
                    {
                        CurrentBindInfo = bindInfo;
                        CurrentUserProfile = GetUserProfile();
                        if (infoError is null)
                        {
                            serviceDelegate?.OnLoginSuccess(token);
                            OnLoginEvents?.Invoke(token);
                        }
                        else
                        {
                            serviceDelegate?.OnLoginFailure(infoError);
                        }
                    });
                }
                else if (error.Code == 0)
                {
                    serviceDelegate?.OnLoginCancel();
                }
                else
                {
                    serviceDelegate?.OnLoginFailure(error);
                }

            });
        }

        public void Login(Action<AccessToken> onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler)
        {
            LoginService.StartLogin((token, error) =>
            {
                if (error is null)
                {
                    Service.Bind.FetchBindInfo((bindInfo, infoError) =>
                    {
                        CurrentBindInfo = bindInfo;
                        CurrentUserProfile = GetUserProfile();

                        if (infoError is null)
                        {
                            onSuccessHandler?.Invoke(token);
                            OnLoginEvents?.Invoke(token);
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

        public void Logout()
        {
            CurrentBindInfo = null;
            CurrentUserProfile = null;

            Service.Login.Logout();
            OnLogoutEvents?.Invoke();
        }

        public void OnCommit(string sex, string date)
        {
            Loader.ShowIndicator();

            Service.Login.CommitPrivateInfo(date, sex, (_, error) =>
            {
                Loader.HideIndicator();

                if (error == null)
                {
                    CurrentUserProfile = GetUserProfile();

                    var info = new UserPrivateInfo();
                    info.Birthday = date;
                    info.Gender = sex;

                    UIService.AdditionalInfo.Close();

                    PrivateInfoTrack.SuccessResult();

                    UserInfoDelegate?.OnConsentAuthPrivateInfo(info);
                    UserInfoDelegate = null;
                }
                else
                {
                    Toast.ShowFail(error.Message);
                    PrivateInfoTrack.FailureResult(error);
                }
            });
        }

        public void OnNextTime()
        {
            UIService.AdditionalInfo.Close();

            PrivateInfoTrack.Cancel();

            UserInfoDelegate?.OnNextTime();
            UserInfoDelegate = null;
        }

        public void OnShowDateView(string date)
        {
            Service.Common.ShowDatePicker(date, (selectDate, _) =>
            {
                if (string.IsNullOrEmpty(selectDate)) { return; }

                UIService.AdditionalInfo.SetDateValue(selectDate);

            });
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

                                    CurrentUserProfile = GetUserProfile();
                                    CurrentBindInfo = info;

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
                        CurrentUserProfile = GetUserProfile();
                        CurrentBindInfo = info;

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

