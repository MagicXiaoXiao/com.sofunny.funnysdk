using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoFunny.FunnySDK.UIModule
{
    internal class UILoginManager : IServiceLoginView
    {
        private GameObject LoginPrefab;
        private SDKUILoginController Controller;
        private HashSet<LoginProvider> Providers;
        private ILoginViewEvent ViewDelegate;

        internal bool Display = false;

        internal UILoginManager()
        {
            LoginPrefab = Resources.Load<GameObject>("FunnySDK/UI/LoginView/LoginController");
        }

        private void Prepare()
        {
            if (!Display)
            {
                GameObject instance = Object.Instantiate(LoginPrefab, UIController.Instance.UIContainer.transform);
                instance.name = "LoginController";
                Controller = instance.GetComponent<SDKUILoginController>();
                Controller.manager = this;
                Controller.SetLoginProviders(Providers);
                Controller.SetLoginViewEvent(ViewDelegate);
                Display = true;
            }
        }

        public void JumpTo(UILoginPageState pageState)
        {
            Prepare();
            Controller.OpenPage(pageState);
        }

        public void Open()
        {
            Prepare();
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

        public void TimerSending(UILoginPageState pageState)
        {
            // 发送中
            Controller.UpdateTimerToSending(pageState);
        }

        public void TimerStart(UILoginPageState pageState)
        {
            // 开始倒计时
            Controller.UpdateTimerToStarted(pageState);
        }

        public void TimerReset(UILoginPageState pageState)
        {
            // 还原倒计时
            Controller.UpdateTimerToReset(pageState);
        }

        public void CloseView()
        {
            if (Controller == null) { return; }

            Controller.CloseLoginController(false);
        }

        public void SetupLoginConfig(ILoginViewEvent loginViewEvent, HashSet<LoginProvider> providers)
        {
            ViewDelegate = loginViewEvent;
            Providers = providers;
        }

    }
}

