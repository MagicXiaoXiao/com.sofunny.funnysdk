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
            LoginView.OnCancelAction?.Invoke(UILoginPageState.AntiAddictionPage);
        }

        private void OnCommitAction()
        {
            // 数据逻辑校验
            string name = nameInputField.text;
            string cardID = idInputField.text;

            Toast.ShowFail("实名认证功能暂未开发");
        }

        private void OnOtherLoginAction()
        {
            LoginView.OnSwitchOtherAction?.Invoke();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {

        }
    }
}

