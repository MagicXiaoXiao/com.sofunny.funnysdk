using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUILoginSelectView : SDKUILoginBase
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


        private ILoginViewEvent loginViewEvent;
        private HashSet<LoginProvider> Providers = new HashSet<LoginProvider>();

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
                var anyOther = Providers.Where((item) =>
                {
                    return item != LoginProvider.Phone &&
                           item != LoginProvider.Email &&
                           item != LoginProvider.Guest;
                });

                providerLoginContainer.SetActive(anyOther.Any());

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
            if (providers is null) return;

            Providers = providers;
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
            LoginView.OnCancelAction?.Invoke(UILoginPageState.LoginSelectPage);
        }

        private bool CheckAgreement()
        {
            if (ConfigService.Config.IsMainland)
            {
                if (!protocolToggle.isOn)
                {
                    Toast.ShowFail(Locale.LoadText("form.protocol.tips"), 1f);
                }
                return protocolToggle.isOn;
            }

            return true;
        }

        #region 登录触发处理

        private void OnSoFunnyLoginAction()
        {
            if (!CheckAgreement()) { return; }
            string account = FunnyDataStore.GetAccountHistory().DefaultIfEmpty("").First();
            Controller.OpenPage(UILoginPageState.PwdLoginPage, account);
        }

        private void OnGuestLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.Guest);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Guest);
        }

        private void OnFacebookLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.Facebook);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Facebook);
        }

        private void OnTwitterLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.Twitter);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Twitter);
        }

        private void OnGoogleLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.Google);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Google);
        }

        private void OnAppleLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.Apple);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.Apple);
        }

        private void OnQQLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.QQ);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.QQ);
        }

        private void OnWeChatLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.WeChat);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.WeChat);
        }

        private void OnTapTapLoginAction()
        {
            if (!CheckAgreement()) { return; }
            LoginView.OnLoginWithProviderAction?.Invoke(LoginProvider.TapTap);
            loginViewEvent?.OnLoginWithProvider(LoginProvider.TapTap);
        }

        #endregion

        private void OnClickAgreementAction()
        {
            LoginView.OnClickUserAgreenment?.Invoke();
            loginViewEvent?.OnClickUserAgreenment();
        }

        private void OnClickPrivacyProtocolAction()
        {
            LoginView.OnClickPriacyProtocol?.Invoke();
            loginViewEvent?.OnClickPriacyProtocol();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}


