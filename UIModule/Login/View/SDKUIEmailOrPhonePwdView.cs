using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIEmailOrPhonePwdView : SDKUILoginBase
    {
        internal override UILoginViewType ViewType => UILoginViewType.EmailOrPhonePwd;

        public InputField emailInputField;
        public InputField pwdInputField;
        public Button retrieveButton;
        public Button registerButton;
        public Button loginButton;
        public Button smsOrPwdButton;
        public Button closeButton;
        public Button backButton;

        public GameObject verifyCodeContainer;
        public InputField smsInputField;
        public Button smsButton;

        private bool isPwd = true;

        protected override void Init()
        {
            retrieveButton.onClick.AddListener(OnRetrieveAction);
            registerButton.onClick.AddListener(OnRegisterAction);
            loginButton.onClick.AddListener(OnLoginAction);
            smsOrPwdButton.onClick.AddListener(OnSmsOrPwdSwitchAction);
            closeButton.onClick.AddListener(OnCloseViewAction);
            backButton.onClick.AddListener(OnBackViewAction);

            smsButton.onClick.AddListener(OnSendSMSAction);
        }

        protected override void DeInit()
        {
            retrieveButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
            loginButton.onClick.RemoveAllListeners();
            smsOrPwdButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
            smsButton.onClick.RemoveAllListeners();
        }

        private void OnRetrieveAction()
        {
            Controller.OpenLoginView(UILoginViewType.RegisterAndRetrieve);
        }

        private void OnRegisterAction()
        {
            Controller.OpenLoginView(UILoginViewType.RegisterAndRetrieve);
        }

        private void OnLoginAction()
        {
            Toast.Show("开发中");
        }

        private void OnSmsOrPwdSwitchAction()
        {
            if (isPwd)
            {
                // 验证码
                pwdInputField.gameObject.SetActive(false);
                verifyCodeContainer.SetActive(true);
                smsOrPwdButton.GetComponentInChildren<Text>().text = "账号密码登录";
            }
            else
            {
                // 密码
                pwdInputField.gameObject.SetActive(true);
                verifyCodeContainer.SetActive(false);
                smsOrPwdButton.GetComponentInChildren<Text>().text = "验证码登录";
            }

            isPwd = !isPwd;
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnBackViewAction()
        {
            Controller.OpenLoginView(UILoginViewType.LoginSelect);
        }

        private void OnSendSMSAction()
        {
            Toast.Show("开发中");
        }
    }
}

