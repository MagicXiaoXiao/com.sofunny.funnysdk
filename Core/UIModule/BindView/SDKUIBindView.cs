using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{

    internal class SDKUIBindView : MonoBehaviour
    {
        public Button closeButton;
        public Button bindButton;
        public Button sendCodeButton;
        public SFSmsCodeButtonTimerHandler timerHandler;
        public InputField emailInputField;
        public InputField pwdInputField;
        public InputField codeInputField;

        private string _bindCode;

        void Awake()
        {
            closeButton.onClick.AddListener(Cancel);
            bindButton.onClick.AddListener(StartBinding);
            sendCodeButton.onClick.AddListener(SendSmsCode);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        internal void SetBindCode(string bindCode)
        {
            _bindCode = bindCode;
            closeButton.gameObject.SetActive(false);
            pwdInputField.gameObject.SetActive(false);
        }

        private void Cancel()
        {
            BindView.OnCancelAction?.Invoke();
            BindView.Close();
        }

        private void SendSmsCode()
        {
            string account = emailInputField.text.Trim();

            if (BridgeConfig.IsMainland)
            {
                if (string.IsNullOrEmpty(_bindCode))
                {
                    Toast.ShowFail("国内版本绑定功能暂未实现");
                }
                else
                {
                    SendBindPhoneCode(account);
                }
            }
            else
            {
                SendEmailCode(account);
            }
        }


        private void SendEmailCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Toast.ShowFail(Locale.LoadText("form.email.required"));
                return;
            }

            if (!email.IsMatchEmail())
            {
                Toast.ShowFail(Locale.LoadText("form.email.verify"));
                return;
            }

            timerHandler.SendingStatus();
            UserProfile profile = Funny.Account.GetUserProfile();

            Internal.CodeAction action = profile.IsGuest ? Internal.CodeAction.GuestBindEmail : Internal.CodeAction.ConfirmEmail;

            Funny.Core.Bridge.Common.SendVerificationCode(email, action, Internal.CodeCategory.Email)
                                    .Then(() =>
                                    {
                                        timerHandler.StartTimer();
                                    })
                                    .Catch((error) =>
                                    {
                                        Toast.ShowFail(error.Message);
                                        timerHandler.ResetTimer();
                                    });
        }

        private void SendBindPhoneCode(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                Toast.ShowFail(Locale.LoadText("form.phone.required"));
                return;
            }

            if (!phone.IsMatchPhone())
            {
                Toast.ShowFail(Locale.LoadText("form.phone.verify"));
                return;
            }

            timerHandler.SendingStatus();

            Funny.Core.Bridge.Common.SendVerificationCode(phone, Internal.CodeAction.LoginBindAccount, Internal.CodeCategory.Phone)
                                    .Then(timerHandler.StartTimer)
                                    .Catch((error) =>
                                    {
                                        Toast.ShowFail(error.Message);
                                        timerHandler.ResetTimer();
                                    });

        }


        private void StartBinding()
        {
            string account = emailInputField.text.Trim();
            string password = pwdInputField.text.Trim();
            string code = codeInputField.text.Trim();

            if (BridgeConfig.IsMainland)
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail(Locale.LoadText("form.phone.required"));
                    return;
                }

                if (!account.IsMatchPhone())
                {
                    Toast.ShowFail(Locale.LoadText("form.phone.verify"));
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(account))
                {
                    Toast.ShowFail(Locale.LoadText("form.email.required"));
                    return;
                }

                if (!account.IsMatchEmail())
                {
                    Toast.ShowFail(Locale.LoadText("form.email.verify"));
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    Toast.ShowFail(Locale.LoadText("form.password.required"));
                    return;
                }

                if (password.Length < 8)
                {
                    Toast.ShowFail(Locale.LoadText("form.password.length.require"));
                    return;
                }
            }

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("form.code.required"));
                return;
            }

            if (string.IsNullOrEmpty(_bindCode))
            {
                BindView.OnCommitAction?.Invoke(account, password, code);
            }
            else
            {
                BindView.OnCommitAction?.Invoke(account, _bindCode, code);
            }

        }

        internal void HideAndClose()
        {
            gameObject.SetActive(false);
            BindView.isLoaded = false;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            sendCodeButton.onClick.RemoveAllListeners();
            bindButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }

    }

}
