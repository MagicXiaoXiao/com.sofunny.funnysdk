using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{

    public class SDKUIEmailOrPhonePwdView : SDKUILoginBase
    {

        public InputField emailOrPhoneInputField;
        public Text emailOrPhonePlaceholder;
        public GameObject pwdContainer;
        public InputField pwdInputField;
        public Button retrieveButton;
        public Button registerButton;
        public Button loginButton;
        public Button smsOrPwdButton;
        public Button closeButton;
        public Button backButton;

        public GameObject verifyCodeContainer;
        public SFSmsCodeButtonTimerHandler timerHandler;
        public InputField smsInputField;
        public Button smsButton;

        internal ILoginViewEvent loginViewEvent;
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

        public override void Show()
        {
            if (ConfigService.Config.IsMainland)
            {
                emailOrPhonePlaceholder.text = "手机号";
                emailOrPhoneInputField.contentType = InputField.ContentType.IntegerNumber;
                emailOrPhoneInputField.characterLimit = 11;
            }
            else
            {
                emailOrPhonePlaceholder.text = "邮箱";
                emailOrPhoneInputField.contentType = InputField.ContentType.EmailAddress;
                emailOrPhoneInputField.characterLimit = 0;
            }

            base.Show();
        }

        private void OnRetrieveAction()
        {
            Controller.OpenPage(UILoginPageState.RetrievePage);
        }

        private void OnRegisterAction()
        {
            Controller.OpenPage(UILoginPageState.RegisterPage);
        }

        private void OnLoginAction()
        {
            // 验证逻辑代码

            // 发起登录
            if (isPwd)
            {
                string account = emailOrPhoneInputField.text;
                string pwd = pwdInputField.text;

                loginViewEvent?.OnLoginWithPassword(account, pwd);
            }
            else
            {
                string account = emailOrPhoneInputField.text;
                string code = smsInputField.text;

                loginViewEvent?.OnLoginWithCode(account, code);
            }

        }

        private void OnSmsOrPwdSwitchAction()
        {
            if (isPwd)
            {
                // 验证码
                pwdContainer.SetActive(false);
                verifyCodeContainer.SetActive(true);
                smsOrPwdButton.GetComponentInChildren<Text>().text = "账号密码登录";
            }
            else
            {
                // 密码
                pwdContainer.SetActive(true);
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
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

        private void OnSendSMSAction()
        {
            // 数据格式效验逻辑
            UILoginPageState page = ConfigService.Config.IsMainland ? UILoginPageState.PhoneLoginPage : UILoginPageState.EmailLoginPage;
            string account = emailOrPhoneInputField.text;
            loginViewEvent?.OnSendVerifcationCode(account, page);
        }

        internal void TimerSending()
        {
            timerHandler.SendingStatus();
        }

        internal void TimerStart()
        {
            timerHandler.StartTimer();
        }

        internal void TimerReset()
        {
            timerHandler.ResetTimer();
        }

    }
}

