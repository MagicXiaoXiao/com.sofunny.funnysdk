using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUICoolDownTipsView : SDKUILoginBase
    {
        public Button closeButton;
        public Text contentLabel;
        public Button recallButton;

        internal ILoginViewEvent loginViewEvent;

        protected override void Init()
        {
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

