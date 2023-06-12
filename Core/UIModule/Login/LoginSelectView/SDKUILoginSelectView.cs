using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUILoginSelectView : SDKUILoginBase
    {

        public Button closeButton;
        public Button sofunnyLoginButton;
        public Button guestLoginButton;
        public Button agreementButton;
        public Button privacyProtocolButton;

        internal ILoginViewEvent loginViewEvent;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            sofunnyLoginButton.onClick.AddListener(OnSoFunnyLoginAction);
            guestLoginButton.onClick.AddListener(OnGuestLoginAction);
            agreementButton.onClick.AddListener(OnClickAgreementAction);
            privacyProtocolButton.onClick.AddListener(OnClickPrivacyProtocolAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            sofunnyLoginButton.onClick.RemoveAllListeners();
            guestLoginButton.onClick.RemoveAllListeners();
            agreementButton.onClick.RemoveAllListeners();
            privacyProtocolButton.onClick.RemoveAllListeners();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnSoFunnyLoginAction()
        {
            if (ConfigService.Config.IsMainland)
            {
                Controller.OpenPage(UILoginPageState.PhoneLoginPage);
            }
            else
            {
                Controller.OpenPage(UILoginPageState.EmailLoginPage);
            }
        }

        private void OnGuestLoginAction()
        {
            Toast.Show("开发中");
        }

        private void OnClickAgreementAction()
        {
            loginViewEvent?.OnClickUserAgreenment();
        }

        private void OnClickPrivacyProtocolAction()
        {
            loginViewEvent?.OnClickPriacyProtocol();
        }
    }
}


