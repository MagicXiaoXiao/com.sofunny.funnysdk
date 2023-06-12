using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUILoginLimitView : SDKUILoginBase
    {

        public Button closeButton;
        public Text contentText;
        public Button otherLoginButton;
        public Button contactButton;

        internal ILoginViewEvent loginViewEvent;

        protected override void Init()
        {
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
            Toast.Show("开发中");
        }
    }
}

