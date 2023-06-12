using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUILoginController : MonoBehaviour
    {

        public SDKUILoginSelectView loginSelectView;
        public SDKUIRegisterAndRetrieveView registerAndRetrieveView;
        public SDKUIEmailOrPhonePwdView emailOrPhonePwdView;
        public SDKUILoginLimitView loginLimitView;
        public SDKUICoolDownTipsView coolDownTipsView;
        public SDKUIActivationKeyView activationKeyView;
        public SDKUIAntiAddictionView antiAddictionView;

        internal UILoginManager manager;
        private ILoginViewEvent loginViewEvent;
        private UILoginPageState currentPageState;

        private void Awake()
        {
            HideAllView();
        }

        private void OnDestroy()
        {
            manager.Display = false;
        }

        internal void SetLoginViewEvent(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
            loginSelectView.loginViewEvent = loginViewEvent;
            registerAndRetrieveView.loginViewEvent = loginViewEvent;
            emailOrPhonePwdView.loginViewEvent = loginViewEvent;
            loginLimitView.loginViewEvent = loginViewEvent;
            coolDownTipsView.loginViewEvent = loginViewEvent;
            activationKeyView.loginViewEvent = loginViewEvent;
            antiAddictionView.loginViewEvent = loginViewEvent;
        }

        public void OpenPage(UILoginPageState pageState)
        {
            if (currentPageState == pageState) { return; }

            HideCurrentPage();

            switch (pageState)
            {
                case UILoginPageState.LoginSelectPage:
                    loginSelectView.Show();
                    break;
                case UILoginPageState.EmailLoginPage:
                    emailOrPhonePwdView.Show();
                    break;
                case UILoginPageState.RegisterPage:
                    registerAndRetrieveView.Show(true);
                    break;
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.Show(false);
                    break;
                case UILoginPageState.LoginLimitPage:
                    loginLimitView.Show();
                    break;
                case UILoginPageState.ActivationKeyPage:
                    activationKeyView.Show();
                    break;
                case UILoginPageState.AntiAddictionPage:
                    antiAddictionView.Show();
                    break;
                case UILoginPageState.CoolDownTipsPage:
                    coolDownTipsView.Show();
                    break;
                case UILoginPageState.PhoneLoginPage:
                    emailOrPhonePwdView.Show();
                    break;
                default:
                    Logger.LogWarning("无法打开未知页面");
                    break;
            }

            currentPageState = pageState;
        }

        public void CloseLoginController()
        {
            gameObject.SetActive(false);
            loginViewEvent?.OnCloseView(currentPageState);
            Destroy(gameObject);
        }

        private void HideAllView()
        {
            loginSelectView.gameObject.SetActive(false);
            registerAndRetrieveView.gameObject.SetActive(false);
            emailOrPhonePwdView.gameObject.SetActive(false);
            loginLimitView.gameObject.SetActive(false);
            coolDownTipsView.gameObject.SetActive(false);
            activationKeyView.gameObject.SetActive(false);
            antiAddictionView.gameObject.SetActive(false);
        }

        private void HideCurrentPage()
        {

            switch (currentPageState)
            {
                case UILoginPageState.LoginSelectPage:
                    loginSelectView.Hide();
                    break;
                case UILoginPageState.EmailLoginPage:
                    emailOrPhonePwdView.Hide();
                    break;
                case UILoginPageState.RegisterPage:
                    registerAndRetrieveView.Hide();
                    break;
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.Hide();
                    break;
                case UILoginPageState.LoginLimitPage:
                    loginLimitView.Hide();
                    break;
                case UILoginPageState.ActivationKeyPage:
                    activationKeyView.Hide();
                    break;
                case UILoginPageState.AntiAddictionPage:
                    antiAddictionView.Hide();
                    break;
                case UILoginPageState.CoolDownTipsPage:
                    coolDownTipsView.Hide();
                    break;
                case UILoginPageState.PhoneLoginPage:
                    emailOrPhonePwdView.Hide();
                    break;

                default: break;
            }
        }

        internal void UpdateTimerToSending(UILoginPageState pageState)
        {
            switch (pageState)
            {
                case UILoginPageState.RegisterPage:
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.TimerSending();
                    break;
                case UILoginPageState.EmailLoginPage:
                case UILoginPageState.PhoneLoginPage:
                    emailOrPhonePwdView.TimerSending();
                    break;
                default:
                    break;
            }
        }

        internal void UpdateTimerToStarted(UILoginPageState pageState)
        {
            switch (pageState)
            {
                case UILoginPageState.RegisterPage:
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.TimerStart();
                    break;
                case UILoginPageState.EmailLoginPage:
                case UILoginPageState.PhoneLoginPage:
                    emailOrPhonePwdView.TimerStart();
                    break;
                default:
                    break;
            }
        }

        internal void UpdateTimerToReset(UILoginPageState pageState)
        {
            switch (pageState)
            {
                case UILoginPageState.RegisterPage:
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.TimerReset();
                    break;
                case UILoginPageState.EmailLoginPage:
                case UILoginPageState.PhoneLoginPage:
                    emailOrPhonePwdView.TimerReset();
                    break;
                default:
                    break;
            }
        }

    }
}


