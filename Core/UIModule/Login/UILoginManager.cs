using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoFunny.FunnySDK.UIModule
{
    internal class UILoginManager : IServiceLoginView
    {
        private readonly GameObject Container;
        private readonly GameObject LoginPrefab;
        private SDKUILoginController Controller;
        private HashSet<LoginProvider> LoginProviders;
        private ILoginViewEvent LoginViewEvent;

        internal bool Display = false;

        internal UILoginManager(GameObject container)
        {
            Container = container;
            LoginPrefab = Resources.Load<GameObject>("FunnySDK/UI/LoginView/LoginController");
        }

        private void Prepare()
        {
            if (!Display)
            {
                GameObject instance = Object.Instantiate(LoginPrefab, Container.transform);
                instance.name = "LoginController";
                Controller = instance.GetComponent<SDKUILoginController>();
                Controller.SetLoginProviders(LoginProviders);
                Controller.SetLoginConfig(LoginViewEvent);
                Display = true;
            }
        }

        public void JumpTo(UILoginPageState pageState, object param = null)
        {
            Prepare();
            Controller?.OpenPage(pageState, param);
        }

        public void Open()
        {
            Prepare();
            Controller?.OpenPage(UILoginPageState.LoginSelectPage);
        }

        public void TimerSending(UILoginPageState pageState)
        {
            // 发送中
            Controller?.UpdateTimerToSending(pageState);
        }

        public void TimerStart(UILoginPageState pageState)
        {
            // 开始倒计时
            Controller?.UpdateTimerToStarted(pageState);
        }

        public void TimerReset(UILoginPageState pageState)
        {
            // 还原倒计时
            Controller?.UpdateTimerToReset(pageState);
        }

        public void CloseView()
        {
            Display = false;

            Controller?.CloseLoginController(false);
            Controller = null;
            LoginViewEvent = null;
        }

        public void SetupLoginConfig(ILoginViewEvent loginViewEvent, HashSet<LoginProvider> providers)
        {
            LoginViewEvent = loginViewEvent;
            LoginProviders = providers;
        }

    }
}

