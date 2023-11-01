using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUILoginController : MonoBehaviour
    {

        public SDKUILoginSelectView loginSelectView;
        public SDKUIRegisterAndRetrieveView registerAndRetrieveView;
        public SDKUIEmailOrPhonePwdView emailOrPhonePwdView;
        public SDKUILoginLimitView loginLimitView;
        public SDKUICoolDownTipsView coolDownTipsView;
        public SDKUIActivationKeyView activationKeyView;
        public SDKUIAntiAddictionView antiAddictionView;

        /// <summary>
        /// 用户主动取消行为
        /// </summary>
        internal Action<UILoginPageState> OnCloseViewAction;

        private ILoginViewEvent loginViewEvent;
        private UILoginPageState currentPageState = UILoginPageState.UnknownPage;

        private void Awake()
        {
            HideAllView();
        }

        internal void SetLoginProviders(HashSet<LoginProvider> providers)
        {
            loginSelectView.SetProvider(providers);
        }

        internal void SetLoginConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
            loginSelectView.SetConfig(loginViewEvent);
            registerAndRetrieveView.SetConfig(loginViewEvent);
            emailOrPhonePwdView.SetConfig(loginViewEvent);
            loginLimitView.SetConfig(loginViewEvent);
            coolDownTipsView.SetConfig(loginViewEvent);
            activationKeyView.SetConfig(loginViewEvent);
            antiAddictionView.SetConfig(loginViewEvent);
        }

        public void OpenPage(UILoginPageState pageState, object param = null)
        {
            if (currentPageState == pageState) { return; }

            HideCurrentPage();

            switch (pageState)
            {
                case UILoginPageState.LoginSelectPage:
                    loginSelectView.Show();
                    break;
                case UILoginPageState.PwdLoginPage:
                    if (param is null)
                    {
                        emailOrPhonePwdView.Show(true);
                    }
                    else
                    {
                        emailOrPhonePwdView.Show(true, (string)param);
                    }
                    break;
                case UILoginPageState.CodeLoginPage:
                    emailOrPhonePwdView.Show(false);
                    break;
                case UILoginPageState.RegisterPage:
                    registerAndRetrieveView.Show(true);
                    break;
                case UILoginPageState.RetrievePage:
                    registerAndRetrieveView.Show(false);
                    break;
                case UILoginPageState.LoginLimitPage:
                    loginLimitView.Content = (string)param;
                    loginLimitView.Show();
                    break;
                case UILoginPageState.ActivationKeyPage:
                    activationKeyView.Show();
                    break;
                case UILoginPageState.AntiAddictionPage:
                    antiAddictionView.Show();
                    break;
                case UILoginPageState.CoolDownTipsPage:
                    coolDownTipsView.Content = (string)param;
                    coolDownTipsView.Show();
                    break;
                default:
                    Logger.LogWarning("无法打开未知页面");
                    break;
            }
            //LoginView.OnOpenViewAction?.Invoke(pageState, currentPageState);
            loginViewEvent?.OnOpenView(pageState, currentPageState);

            currentPageState = pageState;
        }

        internal void CloseLoginController()
        {
            gameObject?.SetActive(false);
            LoginView.isLoaded = false;

            OnCloseViewAction?.Invoke(currentPageState);
            OnCloseViewAction = null;

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
                case UILoginPageState.PwdLoginPage:
                case UILoginPageState.CodeLoginPage:
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
                case UILoginPageState.CodeLoginPage:
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
                case UILoginPageState.CodeLoginPage:
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
                case UILoginPageState.CodeLoginPage:
                    emailOrPhonePwdView.TimerReset();
                    break;
                default:
                    break;
            }
        }

    }
}


