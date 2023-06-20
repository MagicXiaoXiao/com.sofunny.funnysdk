using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUIActivationKeyView : SDKUILoginBase
    {
        public Button closeButton;
        public InputField activationKeyInputField;
        public Button commitButton;

        private ILoginViewEvent loginViewEvent;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            commitButton.onClick.AddListener(OnCommitAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            commitButton.onClick.RemoveAllListeners();
        }

        private void OnCommitAction()
        {
            string code = activationKeyInputField.text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail("请输入邀请码");
                return;
            }

            loginViewEvent?.OnActivationCodeCommit(code);
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

