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

        public GameObject protocolContainer;
        public Toggle protocolToggle;
        public Button agreementButton;
        public Button privacyProtocolButton;

        public GameObject providerLoginContainer;
        public Button facebookButton;
        public Button twitterButton;
        public Button googleButton;
        public Button appleButton;
        public Button wechatButton;
        public Button qqButton;
        public Button taptapButton;


        internal ILoginViewEvent loginViewEvent;
        private HashSet<LoginProvider> Providers;

        protected override void Init()
        {
            HideAllProvider();
            SetupUI();

            protocolContainer.SetActive(ConfigService.Config.IsMainland);

            closeButton.onClick.AddListener(OnCloseViewAction);
            sofunnyLoginButton.onClick.AddListener(OnSoFunnyLoginAction);
            guestLoginButton.onClick.AddListener(OnGuestLoginAction);
            facebookButton.onClick.AddListener(OnFacebookLoginAction);
            twitterButton.onClick.AddListener(OnTwitterLoginAction);
            googleButton.onClick.AddListener(OnGoogleLoginAction);
            appleButton.onClick.AddListener(OnAppleLoginAction);
            wechatButton.onClick.AddListener(OnWeChatLoginAction);
            qqButton.onClick.AddListener(OnQQLoginAction);
            taptapButton.onClick.AddListener(OnTapTapLoginAction);

            agreementButton.onClick.AddListener(OnClickAgreementAction);
            privacyProtocolButton.onClick.AddListener(OnClickPrivacyProtocolAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            sofunnyLoginButton.onClick.RemoveAllListeners();
            guestLoginButton.onClick.RemoveAllListeners();
            facebookButton.onClick.RemoveAllListeners();
            twitterButton.onClick.RemoveAllListeners();
            googleButton.onClick.RemoveAllListeners();
            appleButton.onClick.RemoveAllListeners();
            wechatButton.onClick.RemoveAllListeners();
            qqButton.onClick.RemoveAllListeners();
            taptapButton.onClick.RemoveAllListeners();

            agreementButton.onClick.RemoveAllListeners();
            privacyProtocolButton.onClick.RemoveAllListeners();
        }

        private void SetupUI()
        {
            if (Providers.Count > 0)
            {
                providerLoginContainer.SetActive(true);

                foreach (var provider in Providers)
                {
                    switch (provider)
                    {
                        case LoginProvider.Email:
                        case LoginProvider.Phone:
                            sofunnyLoginButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.Guest:
                            guestLoginButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.Google:
                            googleButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.Apple:
                            appleButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.Facebook:
                            facebookButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.Twitter:
                            twitterButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.QQ:
                            qqButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.WeChat:
                            wechatButton.gameObject.SetActive(true);
                            break;
                        case LoginProvider.TapTap:
                            taptapButton.gameObject.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                HideAllProvider();
            }
        }

        private void HideAllProvider()
        {
            sofunnyLoginButton.gameObject.SetActive(false);
            guestLoginButton.gameObject.SetActive(false);
            providerLoginContainer.SetActive(false);
            facebookButton.gameObject.SetActive(false);
            twitterButton.gameObject.SetActive(false);
            googleButton.gameObject.SetActive(false);
            appleButton.gameObject.SetActive(false);
            wechatButton.gameObject.SetActive(false);
            qqButton.gameObject.SetActive(false);
            taptapButton.gameObject.SetActive(false);
        }

        internal void SetProvider(HashSet<LoginProvider> providers)
        {
            Providers = providers;
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
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Guest);
        }

        private void OnFacebookLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Facebook);
        }

        private void OnTwitterLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Twitter);
        }

        private void OnGoogleLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Google);
        }

        private void OnAppleLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Apple);
        }

        private void OnQQLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.QQ);
        }

        private void OnWeChatLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.WeChat);
        }

        private void OnTapTapLoginAction()
        {
            loginViewEvent?.OnLoginWithProvider(LoginProvider.TapTap);
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


