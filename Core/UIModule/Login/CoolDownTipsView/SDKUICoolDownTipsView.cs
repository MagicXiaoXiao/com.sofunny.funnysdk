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

        internal ILoginViewEvent loginViewEvent;

        internal string Content;

        protected override void Init()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                contentLabel.text = Content;
            }

            closeButton.onClick.AddListener(OnCloseViewAction);
            recallButton.onClick.AddListener(OnRecallAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            recallButton.onClick.RemoveAllListeners();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnRecallAction()
        {
            loginViewEvent?.OnReCallDelete();
        }
    }
}

