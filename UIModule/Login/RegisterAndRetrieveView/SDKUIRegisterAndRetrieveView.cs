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
        public InputField pwdInputField;
        public InputField newPwdInputField;

        public GameObject verifyCodeContainer;
        public SFSmsCodeButtonTimerHandler timerHandler;
        public InputField smsInputField;
        public Button smsButton;

        public Button registerButton;
        public Button retrieveButton;

        protected override void Init()
        {

            backButton.onClick.AddListener(OnBackAction);
            closeButton.onClick.AddListener(OnCloseViewAction);
            smsButton.onClick.AddListener(OnSendSmsAction);
            registerButton.onClick.AddListener(OnRegisterAction);
            retrieveButton.onClick.AddListener(OnRetrieveAction);
        }

        protected override void DeInit()
        {
            backButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            smsButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
            retrieveButton.onClick.RemoveAllListeners();
        }

        internal void Show(bool isRegister)
        {
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
                newPwdInputField.gameObject.SetActive(false);
                retrieveButton.gameObject.SetActive(false);

                verifyCodeContainer.SetActive(true);
                registerButton.gameObject.SetActive(true);
            }
            else
            {
                newPwdInputField.gameObject.SetActive(true);
                retrieveButton.gameObject.SetActive(true);

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
            timerHandler.StartTimer();
        }

        private void OnRegisterAction()
        {
            Toast.Show("开发中");
        }

        private void OnRetrieveAction()
        {
            Toast.Show("开发中");
        }

    }
}

