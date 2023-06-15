using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

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

        private void Start()
        {
            emailOrPhoneInputField.ActivateInputField();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                // 按下了 Tab 键
                if (emailOrPhoneInputField.isFocused)
                {
                    if (isPwd)
                        pwdInputField.ActivateInputField();
                    else
                        smsInputField.ActivateInputField();
                }
                else
                {
                    emailOrPhoneInputField.ActivateInputField();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnLoginAction();
            }

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

        public override void Hide()
        {
            ClearInputFields();

            base.Hide();
        }

        private void OnRetrieveAction()
        {
            Controller.OpenPage(UILoginPageState.RetrievePage);
        }

        private void OnRegisterAction()
        {
            Controller.OpenPage(UILoginPageState.RegisterPage);
        }

        private void ClearInputFields()
        {
            emailOrPhoneInputField.text = "";
            pwdInputField.text = "";
            smsInputField.text = "";
        }

        // 验证账号的方法
        private bool ValidateAccount(string account)
        {
            if (ConfigService.Config.IsMainland)
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail("请填写手机号码");
                    return false;
                }

                if (!account.IsMatchPhone())
                {
                    Toast.ShowFail("手机号格式错误");
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail("请填写邮箱");
                    return false;
                }

                if (!account.IsMatchEmail())
                {
                    Toast.ShowFail("邮箱格式错误");
                    return false;
                }
            }

            return true;
        }

        private void OnLoginAction()
        {
            // 验证逻辑代码
            string account = emailOrPhoneInputField.text.Trim();
            string pwd = pwdInputField.text.Trim();
            string code = smsInputField.text.Trim();

            // 验证账号
            if (!ValidateAccount(account)) { return; }

            // 发起登录
            if (isPwd)
            {
                if (string.IsNullOrEmpty(pwd))
                {
                    Toast.ShowFail("请输入密码");
                    return;
                }

                if (pwd.Length < 8)
                {
                    Toast.ShowFail("密码最少为 8 个字符");
                    return;
                }

                loginViewEvent?.OnLoginWithPassword(account, pwd);
            }
            else
            {
                if (string.IsNullOrEmpty(code))
                {
                    Toast.ShowFail("请输入验证码");
                    return;
                }

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

            // 验证账号
            string account = emailOrPhoneInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

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

