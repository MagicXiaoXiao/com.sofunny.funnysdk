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

        internal FunnyAccountService(FunnyLoginService loginService, BridgeService bridgeService)
        {
            Service = bridgeService;
            LoginService = loginService;
            PrivateInfoTrack = new PrivateInfoAuthTrack(bridgeService.Analysis);
        }

        public void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate)
        {

            Loader.ShowIndicator();
            UserInfoDelegate = serviceDelegate;

            PrivateInfoTrack.Start();

            Service.Login.GetUserProfile((userProfile, error) =>
            {
                Loader.HideIndicator();

                if (error == null)
                {
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
            LoginService.StartLogin(serviceDelegate);
        }

        public void Logout()
        {
            LoginService.Logout();
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

        public void OnShowDateView()
        {
            Service.Common.ShowDatePicker((date, _) =>
            {
                if (string.IsNullOrEmpty(date)) { return; }

                UIService.AdditionalInfo.SetDateValue(date);

            });
        }

    }

}

