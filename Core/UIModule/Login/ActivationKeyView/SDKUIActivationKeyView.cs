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
        public Button otherButton;

        private ILoginViewEvent loginViewEvent;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            commitButton.onClick.AddListener(OnCommitAction);
            otherButton.onClick.AddListener(OnOtherLoginAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            commitButton.onClick.RemoveAllListeners();
            otherButton.onClick.RemoveAllListeners();
        }

        private void OnCommitAction()
        {
            string code = activationKeyInputField.text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("page.activeCode.title"));
                return;
            }
            LoginView.OnCommitActivationAction?.Invoke(code);

            loginViewEvent?.OnActivationCodeCommit(code);
        }

        private void OnOtherLoginAction()
        {
            LoginView.OnSwitchOtherAction?.Invoke();
            loginViewEvent?.OnSwitchOtherAccount();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();

            LoginView.OnCancelAction?.Invoke(UILoginPageState.ActivationKeyPage);
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

