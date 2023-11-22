using System.Collections;
using System.Collections.Generic;
using SoFunny.FunnySDK.Internal;
using UnityEngine;
using UnityEngine.UI;
using static SoFunny.FunnySDK.UIModule.PCLoginPageActions;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUIRegisterOrRetrievePage : MonoBehaviour
    {
        [SerializeField]
        private SFUIInputField accountInputField;
        [SerializeField]
        private SFUIInputField pwdInputField;
        [SerializeField]
        private SFUIInputField codeInputField;
        [SerializeField]
        private Button regCodeButton;
        [SerializeField]
        private Button retCodeButton;
        [SerializeField]
        private SFSmsCodeButtonTimerHandler regTimerHandler;
        [SerializeField]
        private SFSmsCodeButtonTimerHandler retTimerHandler;
        [SerializeField]
        private Button commitButton;

        private bool _isRegister = false;

        internal StringUnityEvent onAccountChangedEvents = new StringUnityEvent();

        private PCLoginPageActions pageActions;

        private void Awake()
        {
            pageActions = new PCLoginPageActions();
            pageActions.Base.SwitchFocus.performed += SwitchFocus_performed;
            pageActions.Base.CommitForm.performed += CommitForm_performed;

            accountInputField.onValueChangedEvents.AddListener((value) => onAccountChangedEvents?.Invoke(value));
            commitButton.onClick.AddListener(OnCommitAction);
            regCodeButton.onClick.AddListener(OnSendCodeAction);
            retCodeButton.onClick.AddListener(OnSendCodeAction);
        }

        private void SwitchFocus_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (accountInputField.isFocused)
            {
                pwdInputField.ActivateInputField();
                return;
            }

            if (pwdInputField.isFocused)
            {
                codeInputField.ActivateInputField();
                return;
            }

            if (codeInputField.isFocused)
            {
                accountInputField.ActivateInputField();
                return;
            }

            if (!accountInputField.isFocused)
            {
                accountInputField.ActivateInputField();
                return;
            }
        }

        private void CommitForm_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            OnCommitAction();
        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
            pageActions.Base.SwitchFocus.performed -= SwitchFocus_performed;
            pageActions.Base.CommitForm.performed -= CommitForm_performed;

            accountInputField.onValueChangedEvents.RemoveAllListeners();
            commitButton.onClick.RemoveAllListeners();
            regCodeButton.onClick.RemoveAllListeners();
            retCodeButton.onClick.RemoveAllListeners();
        }

        private void OnEnable()
        {
            pageActions.Base.Enable();
        }

        private void OnDisable()
        {
            pageActions.Base.Disable();
        }

        internal void OnAccountInputChanged(string value)
        {
            accountInputField.text = value;
        }

        #region Enter And Exit

        internal void EnterRegisterPage()
        {
            pwdInputField.text = "";
            codeInputField.text = "";
            regCodeButton.gameObject.SetActive(true);
            commitButton.GetComponentInChildren<Text>().text = "注册并登录";
            accountInputField.placeholder = "邮箱";
            pwdInputField.placeholder = "密码";
            codeInputField.placeholder = "验证码";

            gameObject.SetActive(true);

            _isRegister = true;
        }

        internal void ExitRegisterPage()
        {
            regCodeButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        internal void EnterRetrievePage()
        {
            pwdInputField.text = "";
            codeInputField.text = "";
            retCodeButton.gameObject.SetActive(true);
            commitButton.GetComponentInChildren<Text>().text = "找回密码";
            accountInputField.placeholder = "邮箱";
            pwdInputField.placeholder = "新密码";
            codeInputField.placeholder = "验证码";

            gameObject.SetActive(true);

            _isRegister = false;
        }

        internal void ExitRetrievePage()
        {
            retCodeButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        #endregion

        private void OnSendCodeAction()
        {
            // 校验账号规则逻辑
            string account = accountInputField.text.Trim();

            if (!ValidateAccount(account)) { return; }

            CodeAction codeAction = _isRegister ? CodeAction.Signup : CodeAction.ChangePassword;
            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
            UILoginPageState pageState = _isRegister ? UILoginPageState.RegisterPage : UILoginPageState.RetrievePage;

            SFSmsCodeButtonTimerHandler current = _isRegister ? regTimerHandler : retTimerHandler;

            current.SendingStatus();

            Funny.Core.Bridge.Common.SendVerificationCode(account, codeAction, category)
                                    .Then(() =>
                                    {
                                        PCLoginView.OnSendVerifcationCodeAction?.Invoke(pageState, null);
                                        current.StartTimer();
                                    })
                                    .Catch((error) =>
                                    {
                                        PCLoginView.OnSendVerifcationCodeAction?.Invoke(pageState, (ServiceError)error);
                                        current.ResetTimer();
                                    });
        }

        private void OnCommitAction()
        {
            // 校验账号规则逻辑
            string account = accountInputField.text.Trim();
            string pwd = pwdInputField.text.Trim();
            string code = codeInputField.text.Trim();

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

            if (_isRegister)
            {
                PCLoginView.OnRegisterAccountAction?.Invoke(account, pwd, code);
            }
            else
            {
                PCLoginView.OnRetrievePasswordAction?.Invoke(account, pwd, code);
            }

        }

        // 验证账号的方法
        private bool ValidateAccount(string account)
        {
            if (BridgeConfig.IsMainland)
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

    }
}