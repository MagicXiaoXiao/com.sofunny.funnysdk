using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUIAntiAddictionView : SDKUILoginBase
    {
        public Button closeButton;
        public InputField nameInputField;
        public InputField idInputField;
        public Button commitButton;
        public Button otherLoginButton;

        internal ILoginViewEvent loginViewEvent;

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
            // 数据逻辑校验
            string name = nameInputField.text;
            string cardID = idInputField.text;

            loginViewEvent?.OnRealnameInfoCommit(name, cardID);
        }

        private void OnOtherLoginAction()
        {
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

    }
}

