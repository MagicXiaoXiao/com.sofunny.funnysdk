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
        private PrivateInfoAuthTrack PrivateInfoTrack;

        public event Action OnLogoutEvents;
        public event Action<AccessToken> OnLoginEvents;

        internal FunnyAccountService(FunnyLoginService loginService, BridgeService bridgeService)
        {
            Service = bridgeService;
            LoginService = loginService;
            PrivateInfoTrack = new PrivateInfoAuthTrack(bridgeService.Analysis);
        }
        // FIXME Android 移动端待处理: Google 账号要从 Google People Api 获取用户年龄性别信息直接返回
        public void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate)
        {
            Logger.Log("发起用户信息授权 - GetPrivateUserInfo");

            Loader.ShowIndicator();
            UserInfoDelegate = serviceDelegate;

            PrivateInfoTrack.Start();

            Service.Login.GetUserProfile((userProfile, error) =>
            {
                Loader.HideIndicator();

                if (error == null)
                {
                    if (userProfile is null)
                    {
                        UserInfoDelegate?.OnPrivateInfoFailure(ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                        return;
                    }

                    if (userProfile.PrivateInfo is null) // 开关判断
                    {
                        PrivateInfoTrack.NotEnabled();

                        UserInfoDelegate?.OnUnenabledService();
                        UserInfoDelegate = null;
                    }
                    else if (userProfile.PrivateInfo.Filled) // 信息完整判断
                    {
                        PrivateInfoTrack.SuccessResult();

                        UserInfoDelegate?.OnConsentAuthPrivateInfo(userProfile.PrivateInfo);
                        UserInfoDelegate = null;
                    }
                    else
                    {
                        FetchPrivateInfoHandler();
                    }
                }
                else
                {
                    PrivateInfoTrack.FailureResult(error);

                    UserInfoDelegate?.OnPrivateInfoFailure(error);
                    UserInfoDelegate = null;
                }
            });
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
            Service.Login.GetUserProfile((userProfile, error) =>
            {
                if (error == null)
                {
                    serviceDelegate?.OnUserProfileSuccess(userProfile);
                }
                else
                {
                    serviceDelegate?.OnUserProfileFailure(error);
                }
            });
        }

        public void Login(ILoginServiceDelegate serviceDelegate)
        {
            LoginService.StartLogin((token, error) =>
            {
                if (error is null)
                {
                    serviceDelegate?.OnLoginSuccess(token);

                    OnLoginEvents?.Invoke(token);
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

        public void Logout()
        {
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

    }

}

