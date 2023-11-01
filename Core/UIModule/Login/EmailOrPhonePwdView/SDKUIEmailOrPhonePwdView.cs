using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK.UIModule
{

    internal class SDKUIEmailOrPhonePwdView : SDKUILoginBase
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

        private ILoginViewEvent loginViewEvent;
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

        private void SwitchAccountInputField()
        {
            if (ConfigService.Config.IsMainland)
            {

                emailOrPhonePlaceholder.text = Locale.LoadText("form.phone.placeholder");
                emailOrPhoneInputField.contentType = InputField.ContentType.IntegerNumber;
                emailOrPhoneInputField.characterLimit = 11;
            }
            else
            {
                emailOrPhonePlaceholder.text = Locale.LoadText("form.email.placeholder");
                emailOrPhoneInputField.contentType = InputField.ContentType.EmailAddress;
                emailOrPhoneInputField.characterLimit = 0;
            }
        }

        public void Show(bool isPwd, string defaultAccount = "")
        {
            this.isPwd = isPwd;

            if (!string.IsNullOrEmpty(defaultAccount))
            {
                this.emailOrPhoneInputField.text = defaultAccount;
            }

            SwitchAccountInputField();

            SwitchPwdOrCodeUI();

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
            pwdInputField.text = "";
            smsInputField.text = "";
        }

        private void SwitchPwdOrCodeUI()
        {
            if (isPwd)
            {
                // 密码
                pwdContainer.SetActive(true);
                verifyCodeContainer.SetActive(false);

                smsOrPwdButton.GetComponentInChildren<Text>().text = Locale.LoadText("page.login.tab.login-code");//"验证码登录";
            }
            else
            {
                // 验证码
                pwdContainer.SetActive(false);
                verifyCodeContainer.SetActive(true);

                smsOrPwdButton.GetComponentInChildren<Text>().text = Locale.LoadText("page.login.tab.login-credentials");//"账号密码登录";
            }
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
                    Toast.ShowFail(Locale.LoadText("form.password.required"));
                    return;
                }

                if (pwd.Length < 8)
                {
                    Toast.ShowFail(Locale.LoadText("form.password.length.require"));
                    return;
                }

                LoginView.OnLoginWithPasswordAction(account, pwd);

                loginViewEvent?.OnLoginWithPassword(account, pwd);
            }
            else
            {
                if (string.IsNullOrEmpty(code))
                {
                    Toast.ShowFail(Locale.LoadText("form.code.required"));
                    return;
                }

                LoginView.OnLoginWithCodeAction(account, code);

                loginViewEvent?.OnLoginWithCode(account, code);
            }

        }

        private void OnSmsOrPwdSwitchAction()
        {
            bool mainland = ConfigService.Config.IsMainland;

            if (isPwd)
            {
                Controller.OpenPage(UILoginPageState.CodeLoginPage);
            }
            else
            {
                Controller.OpenPage(UILoginPageState.PwdLoginPage);
            }

            //isPwd = !isPwd;

            //SwitchPwdOrCodeUI();
        }

        private void OnCloseViewAction()
        {
            Controller.CloseLoginController();

            LoginView.OnCancelAction?.Invoke(isPwd ? UILoginPageState.PwdLoginPage : UILoginPageState.CodeLoginPage);
        }

        private void OnBackViewAction()
        {
            Controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

        private void OnSendSMSAction()
        {
            // 数据格式效验逻辑
            //UILoginPageState page = ConfigService.Config.IsMainland ? UILoginPageState.CodeLoginPage : UILoginPageState.CodeLoginPage;

            // 验证账号
            string account = emailOrPhoneInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

            CodeCategory category = ConfigService.Config.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

            UILoginPageState pageState = isPwd ? UILoginPageState.PwdLoginPage : UILoginPageState.CodeLoginPage;

            timerHandler.SendingStatus();

            Funny.Core.Bridge.Common.SendVerificationCode(account, CodeAction.Login, category)
                                    .Then(() =>
                                    {
                                        timerHandler.StartTimer();
                                        LoginView.OnSendVerifcationCodeAction?.Invoke(pageState, null);
                                    })
                                    .Catch((error) =>
                                    {
                                        timerHandler.ResetTimer();
                                        LoginView.OnSendVerifcationCodeAction?.Invoke(pageState, (ServiceError)error);
                                    });

            //loginViewEvent?.OnSendVerifcationCode(account, UILoginPageState.CodeLoginPage);
        }

        internal void TimerSending()
        {
            //timerHandler.SendingStatus();
        }

        internal void TimerStart()
        {
            //timerHandler.StartTimer();
        }

        internal void TimerReset()
        {
            //timerHandler.ResetTimer();
        }

        public override void SetConfig(ILoginViewEvent loginViewEvent)
        {
            this.loginViewEvent = loginViewEvent;
        }
    }
}

