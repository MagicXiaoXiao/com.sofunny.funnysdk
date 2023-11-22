using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace SoFunny.FunnySDK.UIModule
{
    internal class StringUnityEvent : UnityEvent<string> { }

    internal class PCUILoginViewController : MonoBehaviour
    {
        [SerializeField]
        private Button leftArrowButton;
        [SerializeField]
        private Button closeButton;
        [SerializeField]
        private PCUILoginMainPage loginMainPage;
        [SerializeField]
        private PCUIRegisterOrRetrievePage registerOrRetrievePage;
        [SerializeField]
        private PCUIAccountLimitPage accountLimitPage;
        [SerializeField]
        private PCUIActCodePage actCodePage;
        [SerializeField]
        private PCUICooldownPage cooldownPage;

        private UILoginPageState currentPageState = UILoginPageState.UnknownPage;
        internal LoginProvider[] providers;

        private void Awake()
        {
            loginMainPage.onAccountChangedEvents.AddListener(registerOrRetrievePage.OnAccountInputChanged);
            registerOrRetrievePage.onAccountChangedEvents.AddListener(loginMainPage.OnAccountInputChanged);

            closeButton.onClick.AddListener(OnCancelAction);
            leftArrowButton.onClick.AddListener(OnBackPageAction);
        }

        private void OnDestroy()
        {
            loginMainPage.onAccountChangedEvents.RemoveAllListeners();
            registerOrRetrievePage.onAccountChangedEvents.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            leftArrowButton.onClick.RemoveAllListeners();
        }

        private void OnCancelAction()
        {
            Close();

            PCLoginView.OnCancelAction?.Invoke(currentPageState);
        }

        private void OnBackPageAction()
        {
            switch (currentPageState)
            {
                case UILoginPageState.RegisterPage:
                case UILoginPageState.RetrievePage:
                    Enter(PCLoginPage.LoginWithPassword());
                    break;
                default:
                    break;
            }
        }

        internal void Close()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            PCLoginView.isLoaded = false;
        }

        internal void Enter(PCLoginPage page)
        {

            if (page.PageState == currentPageState) return;

            Exit(currentPageState);

            switch (page.PageState)
            {
                case UILoginPageState.PwdLoginPage:
                    leftArrowButton.gameObject.SetActive(false);
                    loginMainPage.EnterPwdPage();
                    break;
                case UILoginPageState.CodeLoginPage:
                    leftArrowButton.gameObject.SetActive(false);
                    loginMainPage.EnterCodePage();
                    break;
                case UILoginPageState.RegisterPage:
                    leftArrowButton.gameObject.SetActive(true);
                    registerOrRetrievePage.EnterRegisterPage();
                    break;
                case UILoginPageState.RetrievePage:
                    leftArrowButton.gameObject.SetActive(true);
                    registerOrRetrievePage.EnterRetrievePage();
                    break;
                case UILoginPageState.ActivationKeyPage:
                    leftArrowButton.gameObject.SetActive(false);
                    actCodePage.Enter();
                    break;
                case UILoginPageState.LoginLimitPage:
                    leftArrowButton.gameObject.SetActive(false);
                    accountLimitPage.Enter(page.Message);
                    break;
                case UILoginPageState.CoolDownTipsPage:
                    leftArrowButton.gameObject.SetActive(false);
                    cooldownPage.Enter(page.Message);
                    break;
                default:
                    break;
            }

            currentPageState = page.PageState;
        }

        private void Exit(UILoginPageState pageState)
        {
            switch (pageState)
            {
                case UILoginPageState.PwdLoginPage:
                    loginMainPage.ExitPwdPage();
                    break;
                case UILoginPageState.CodeLoginPage:
                    loginMainPage.ExitCodePage();
                    break;
                case UILoginPageState.RegisterPage:
                    registerOrRetrievePage.ExitRegisterPage();
                    break;
                case UILoginPageState.RetrievePage:
                    registerOrRetrievePage.ExitRetrievePage();
                    break;
                case UILoginPageState.ActivationKeyPage:
                    actCodePage.Exit();
                    break;
                case UILoginPageState.LoginLimitPage:
                    accountLimitPage.Exit();
                    break;
                case UILoginPageState.CoolDownTipsPage:
                    cooldownPage.Exit();
                    break;
                default:
                    break;
            }
        }

        internal void ExitAll()
        {
            currentPageState = UILoginPageState.UnknownPage;

            loginMainPage?.ExitCodePage();
            loginMainPage?.ExitPwdPage();

            registerOrRetrievePage?.ExitRegisterPage();
            registerOrRetrievePage?.ExitRetrievePage();

            accountLimitPage?.Exit();
            actCodePage?.Exit();
            cooldownPage?.Exit();
        }

    }
}


