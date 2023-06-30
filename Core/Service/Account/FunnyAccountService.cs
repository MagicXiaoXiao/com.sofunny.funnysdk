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

        internal FunnyAccountService(FunnyLoginService loginService, BridgeService bridgeService)
        {
            Service = bridgeService;
            LoginService = loginService;
        }

        public void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate)
        {
            Loader.ShowIndicator();
            UserInfoDelegate = serviceDelegate;

            Service.Login.GetUserProfile((userProfile, error) =>
            {
                Loader.HideIndicator();

                if (error == null)
                {
                    if (userProfile.PrivateInfo is null) // 开关判断
                    {
                        UserInfoDelegate?.OnUnenabledService();
                        UserInfoDelegate = null;
                    }
                    else if (userProfile.PrivateInfo.Filled) // 信息完整判断
                    {
                        UserInfoDelegate?.OnConsentAuthPrivateInfo(userProfile.PrivateInfo);
                        UserInfoDelegate = null;
                    }
                    else
                    {
                        UIService.AdditionalInfo.Open(this);
                    }
                }
                else
                {
                    UserInfoDelegate?.OnUnenabledService();
                    UserInfoDelegate = null;
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
                    info.Sex = sex;

                    UIService.AdditionalInfo.Close();

                    UserInfoDelegate?.OnConsentAuthPrivateInfo(info);
                    UserInfoDelegate = null;
                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });
        }

        public void OnNextTime()
        {
            UIService.AdditionalInfo.Close();

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

