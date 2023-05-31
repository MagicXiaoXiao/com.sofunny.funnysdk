using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUILoginSelectView : SDKUILoginBase
    {

        public Button closeButton;
        public Button sofunnyLoginButton;
        public Button guestLoginButton;
        public GameObject channelContainer;

        protected override void Init()
        {
            closeButton.onClick.AddListener(OnCloseViewAction);
            sofunnyLoginButton.onClick.AddListener(OnSoFunnyLoginAction);
            guestLoginButton.onClick.AddListener(OnGuestLoginAction);
        }

        protected override void DeInit()
        {
            closeButton.onClick.RemoveAllListeners();
            sofunnyLoginButton.onClick.RemoveAllListeners();
            guestLoginButton.onClick.RemoveAllListeners();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnSoFunnyLoginAction()
        {
            Controller.OpenPage(UILoginPageState.EmailLoginPage);
        }

        private void OnGuestLoginAction()
        {
            Toast.Show("开发中");
        }
    }
}


