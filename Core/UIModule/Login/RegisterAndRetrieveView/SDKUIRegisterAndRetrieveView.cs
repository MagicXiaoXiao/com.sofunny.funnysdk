using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIRegisterAndRetrieveView : SDKUILoginBase
    {
        public Button backButton;
        public Button closeButton;

        public InputField emailOrPhoneInputField;
        public Text emailOrPhonePlaceholder;
        public GameObject pwdContainer;
        public InputField pwdInputField;
        public GameObject newPwdContainer;
        public InputField newPwdInputField;

        public GameObject verifyCodeContainer;
        public SFSmsCodeButtonTimerHandler timerHandler;
        public InputField smsInputField;
        public Button smsButton;

        public GameObject retrieveVCodeContainer;
        public SFSmsCodeButtonTimerHandler retTimerHandler;
        public InputField retSmsInputField;
        public Button retSmsButton;

        public Button registerButton;
        public Button retrieveButton;

        internal ILoginViewEvent loginViewEvent;
        private bool isRegister = false;

        protected override void Init()
        {

            backButton.onClick.AddListener(OnBackAction);
            closeButton.onClick.AddListener(OnCloseViewAction);
            smsButton.onClick.AddListener(OnSendSmsAction);
            retSmsButton.onClick.AddListener(OnSendSmsAction);
            registerButton.onClick.AddListener(OnRegisterAction);
            retrieveButton.onClick.AddListener(OnRetrieveAction);
        }

        protected override void DeInit()
        {
            backButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            smsButton.onClick.RemoveAllListeners();
            retSmsButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
            retrieveButton.onClick.RemoveAllListeners();
        }

        internal void Show(bool isRegister)
        {
            this.isRegister = isRegister;

            if (ConfigService.Config.IsMainland)
            {
                emailOrPhonePlaceholder.text = "手机号";
                emailOrPhoneInputField.characterLimit = 11;
                emailOrPhoneInputField.contentType = InputField.ContentType.IntegerNumber;
            }
            else
            {
                emailOrPhonePlaceholder.text = "邮箱";
                emailOrPhoneInputField.characterLimit = 0;
                emailOrPhoneInputField.contentType = InputField.ContentType.EmailAddress;
            }

            if (isRegister)
            {
                newPwdContainer.SetActive(false);
                newPwdInputField.gameObject.SetActive(false);
                retrieveButton.gameObject.SetActive(false);
                retrieveVCodeContainer.SetActive(false);

                pwdContainer.SetActive(true);
                verifyCodeContainer.SetActive(true);
                registerButton.gameObject.SetActive(true);
            }
            else
            {
                newPwdContainer.SetActive(true);
                newPwdInputField.gameObject.SetActive(true);
                retrieveButton.gameObject.SetActive(true);
                retrieveVCodeContainer.SetActive(true);

                pwdContainer.SetActive(false);
                verifyCodeContainer.SetActive(false);
                registerButton.gameObject.SetActive(false);
            }

            base.Show();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnBackAction()
        {
            Controller.OpenPage(UILoginPageState.EmailLoginPage);
        }

        private void OnSendSmsAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text;
            // 判断是否是注册行为
            loginViewEvent?.OnSendVerifcationCode(account, isRegister ? UILoginPageState.RegisterPage : UILoginPageState.RetrievePage);
        }

        private void OnRegisterAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text;
            string pwd = pwdInputField.text;
            string code = smsInputField.text;

            // 发起账号注册
            loginViewEvent?.OnRegisterAccount(account, pwd, code);
        }

        private void OnRetrieveAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text;
            string newPwd = newPwdInputField.text;
            string code = retSmsInputField.text;

            // 发起账号注册
            loginViewEvent?.OnRetrievePassword(account, newPwd, code);
        }


        internal void TimerSending()
        {
            if (isRegister)
            {
                timerHandler.SendingStatus();
            }
            else
            {
                retTimerHandler.SendingStatus();
            }

        }

        internal void TimerStart()
        {
            if (isRegister)
            {
                timerHandler.StartTimer();
            }
            else
            {
                retTimerHandler.StartTimer();
            }
        }

        internal void TimerReset()
        {
            if (isRegister)
            {
                timerHandler.ResetTimer();
            }
            else
            {
                retTimerHandler.ResetTimer();
            }

        }
    }
}

