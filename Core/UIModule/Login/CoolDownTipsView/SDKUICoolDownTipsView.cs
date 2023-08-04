using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUICoolDownTipsView : SDKUILoginBase
    {
        public Button closeButton;
        public Text contentLabel;
        public Button recallButton;
        public Button switchButton;

        internal ILoginViewEvent loginViewEvent;

        internal string Content;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            recallButton.onClick.AddListener(OnRecallAction);
            switchButton.onClick.AddListener(OnSwitchAccount);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            recallButton.onClick.RemoveAllListeners();
            switchButton.onClick.RemoveAllListeners();
        }

        public override void Show()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                contentLabel.text = Content;
            }

            base.Show();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnRecallAction()
        {
            loginViewEvent?.OnReCallDelete();
        }

        private void OnSwitchAccount()
        {
            loginViewEvent?.OnSwitchOtherAccount();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

