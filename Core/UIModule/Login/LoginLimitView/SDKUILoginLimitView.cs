using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUILoginLimitView : SDKUILoginBase
    {

        public Button closeButton;
        public Text contentText;
        public Button otherLoginButton;
        public Button contactButton;

        private ILoginViewEvent loginViewEvent;
        internal string Content;

        protected override void Init()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                contentText.text = Content;
            }

            closeButton.onClick.AddListener(OnCloseViewAction);
            otherLoginButton.onClick.AddListener(OnOtherLoginAction);
            contactButton.onClick.AddListener(OnContactAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            otherLoginButton.onClick.RemoveAllListeners();
            contactButton.onClick.RemoveAllListeners();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnOtherLoginAction()
        {
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

        private void OnContactAction()
        {
            loginViewEvent?.OnClickContactUS();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

