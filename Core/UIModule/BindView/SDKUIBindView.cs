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

        private void Cancel()
        {
            BindView.OnCancelAction?.Invoke();
            BindView.Close();
        }

        private void SendSmsCode()
        {

            string email = emailInputField.text.Trim();

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

            Funny.Core.Bridge.Common.SendVerificationCode(
                email,
                action,
                Internal.CodeCategory.Email,
                (_, error) =>
                {
                    if (error is null)
                    {
                        timerHandler.StartTimer();
                    }
                    else
                    {
                        Toast.ShowFail(error.Message);
                        timerHandler.ResetTimer();
                    }
                }
            );
        }

        private void StartBinding()
        {
            string email = emailInputField.text.Trim();
            string password = pwdInputField.text.Trim();
            string code = codeInputField.text.Trim();

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

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("form.code.required"));
                return;
            }

            BindView.OnCommitAction?.Invoke(email, password, code);
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
