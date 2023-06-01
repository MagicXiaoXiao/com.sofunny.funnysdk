using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIAntiAddictionView : SDKUILoginBase
    {
        public Button closeButton;
        public InputField nameInputField;
        public InputField idInputField;
        public Button commitButton;
        public Button otherLoginButton;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            commitButton.onClick.AddListener(OnCommitAction);
            otherLoginButton.onClick.AddListener(OnOtherLoginAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            commitButton.onClick.RemoveAllListeners();
            otherLoginButton.onClick.RemoveAllListeners();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnCommitAction()
        {
            Toast.Show("开发中");
        }

        private void OnOtherLoginAction()
        {
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

    }
}

