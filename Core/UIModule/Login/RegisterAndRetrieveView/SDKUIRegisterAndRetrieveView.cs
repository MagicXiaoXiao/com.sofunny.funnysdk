using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUIRegisterAndRetrieveView : SDKUILoginBase
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

        private ILoginViewEvent loginViewEvent;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isRegister)
                {
                    if (emailOrPhoneInputField.isFocused)
                    {
                        pwdInputField.ActivateInputField();
                        return;
                    }

                    if (pwdInputField.isFocused)
                    {
                        smsInputField.ActivateInputField();
                        return;
                    }

                    if (smsInputField.isFocused)
                    {
                        emailOrPhoneInputField.ActivateInputField();
                        return;
                    }

                    if (!emailOrPhoneInputField.isFocused)
                    {
                        emailOrPhoneInputField.ActivateInputField();
                        return;
                    }
                }
                else
                {
                    if (emailOrPhoneInputField.isFocused)
                    {
                        newPwdInputField.ActivateInputField();
                        return;
                    }

                    if (newPwdInputField.isFocused)
                    {
                        retSmsInputField.ActivateInputField();
                        return;
                    }

                    if (retSmsInputField.isFocused)
                    {
                        emailOrPhoneInputField.ActivateInputField();
                        return;
                    }

                    if (!emailOrPhoneInputField.isFocused)
                    {
                        emailOrPhoneInputField.ActivateInputField();
                        return;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (isRegister)
                    OnRegisterAction();
                else
                    OnRetrieveAction();
            }


        }

        internal void Show(bool isRegister)
        {
            this.isRegister = isRegister;

            if (ConfigService.Config.IsMainland)
            {
                emailOrPhonePlaceholder.text = Locale.LoadText("form.phone.placeholder");
                emailOrPhoneInputField.characterLimit = 11;
                emailOrPhoneInputField.contentType = InputField.ContentType.IntegerNumber;
            }
            else
            {
                emailOrPhonePlaceholder.text = Locale.LoadText("form.email.placeholder");
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

        public override void Hide()
        {
            ClearInputFields();

            base.Hide();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();
        }

        private void OnBackAction()
        {
            Controller.OpenPage(UILoginPageState.PwdLoginPage);
        }

        private void ClearInputFields()
        {
            emailOrPhoneInputField.text = "";
            pwdInputField.text = "";
            newPwdInputField.text = "";

            smsInputField.text = "";
            retSmsInputField.text = "";
        }

        // 验证账号的方法
        private bool ValidateAccount(string account)
        {
            if (ConfigService.Config.IsMainland)
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail(Locale.LoadText("form.phone.required"));
                    return false;
                }

                if (!account.IsMatchPhone())
                {
                    Toast.ShowFail(Locale.LoadText("form.phone.verify"));
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail(Locale.LoadText("form.email.required"));
                    return false;
                }

                if (!account.IsMatchEmail())
                {
                    Toast.ShowFail(Locale.LoadText("form.email.verify"));
                    return false;
                }
            }

            return true;
        }

        private void OnSendSmsAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

            loginViewEvent?.OnSendVerifcationCode(account, isRegister ? UILoginPageState.RegisterPage : UILoginPageState.RetrievePage);
        }

        private void OnRegisterAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text.Trim();
            string pwd = pwdInputField.text.Trim();
            string code = smsInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

            if (string.IsNullOrEmpty(pwd))
            {
                Toast.ShowFail(Locale.LoadText("form.password.required"));
                return;
            }

            if (pwd.Length < 8)
            {
                Toast.ShowFail(Locale.LoadText("form.password.length.require"));
                return;
            }

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("form.code.required"));
                return;
            }

            // 发起账号注册
            loginViewEvent?.OnRegisterAccount(account, pwd, code);
        }

        private void OnRetrieveAction()
        {
            // 校验账号规则逻辑
            string account = emailOrPhoneInputField.text.Trim();
            string newPwd = newPwdInputField.text.Trim();
            string code = retSmsInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

            if (string.IsNullOrEmpty(newPwd))
            {
                Toast.ShowFail(Locale.LoadText("form.password.required"));
                return;
            }

            if (newPwd.Length < 8)
            {
                Toast.ShowFail(Locale.LoadText("form.password.length.require"));
                return;
            }

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("form.code.required"));
                return;
            }

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

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

