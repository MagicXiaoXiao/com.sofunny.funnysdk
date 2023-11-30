using System.Collections;
using System.Collections.Generic;
using SoFunny.FunnySDK.Internal;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUILoginMainPage : MonoBehaviour
    {
        [SerializeField]
        private SFUIInputField accountInputField;
        [SerializeField]
        private SFUIInputField pwdInputField;
        [SerializeField]
        private SFUIInputField codeInputField;
        [SerializeField]
        private Button codeButton;
        [SerializeField]
        private Button codeLoginButton;
        [SerializeField]
        private SFSmsCodeButtonTimerHandler timerHandler;

        [SerializeField]
        private GameObject smsContainer;

        [SerializeField]
        private Toggle rememberToggle;
        [SerializeField]
        private Button retrieveButton;
        [SerializeField]
        private Button registerButton;
        [SerializeField]
        private GameObject leftMeun;
        [SerializeField]
        private Button accountPwdButton;

        [SerializeField]
        private GameObject bottomMeun;
        [SerializeField]
        private Toggle agreementToggle;
        [SerializeField]
        private Button userProtocolButton;
        [SerializeField]
        private Button privacyProtocolButton;

        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private PCUIRecordListPage recordListPage;

        private PCLoginPageActions pageActions;

        internal StringUnityEvent onAccountChangedEvents = new StringUnityEvent();

        /// <summary>
        /// 是否是登录记录
        /// </summary>
        private bool IsLoginRecord = false;
        /// <summary>
        /// 是否是密码登录状态
        /// </summary>
        private bool IsPasswordStatus = true;

        private void Awake()
        {
            pageActions = new PCLoginPageActions();
            pageActions.Base.SwitchFocus.performed += SwitchFocus_performed;
            pageActions.Base.CommitForm.performed += CommitForm_performed;

            codeLoginButton.onClick.AddListener(() => PCLoginView.Open(PCLoginPage.LoginWithCode()));
            retrieveButton.onClick.AddListener(() => PCLoginView.Open(PCLoginPage.Retrieve()));
            registerButton.onClick.AddListener(() => PCLoginView.Open(PCLoginPage.Register()));
            accountPwdButton.onClick.AddListener(() => PCLoginView.Open(PCLoginPage.LoginWithPassword()));
            userProtocolButton.onClick.AddListener(() => PCLoginView.OnClickUserAgreenment?.Invoke());
            privacyProtocolButton.onClick.AddListener(() => PCLoginView.OnClickPriacyProtocol?.Invoke());

            accountInputField.onValueChangedEvents.AddListener(OnAccountInputFieldValueChangedAction);
            accountInputField.onValueChangedEvents.AddListener((value) => onAccountChangedEvents?.Invoke(value));

            accountInputField.onClearContentEvents.AddListener(() =>
            {
                pwdInputField.text = "";
                rememberToggle.isOn = false;
                agreementToggle.isOn = false;
            });

            accountInputField.onClickArrowEvents.AddListener(() =>
            {
                if (recordListPage.IsActive)
                {
                    recordListPage.Hide();
                }
                else
                {
                    recordListPage.Show();
                }
            });

            pwdInputField.onClearContentEvents.AddListener(OnCleanAccountInputFieldAction);
            pwdInputField.onValueChangedEvents.AddListener(OnPwdInputValueChangedAction);

            codeButton.onClick.AddListener(OnSendSmsCodeAction);
            loginButton.onClick.AddListener(OnLoginAccountAction);

            recordListPage.onSelectRecordEvents += OnSelectRecordAction;
            recordListPage.onDeleteRecordEvents += OnDeleteRecordAction;
            recordListPage.onEmptyListEvents += OnRecordListEmptyAction;

            PCLoginView.OnInvaidTokenResultAction += OnInvaidTokenAction;
        }

        private void Start()
        {
            if (BridgeConfig.IsMainland)
            {
                accountInputField.SetContentType(InputField.ContentType.IntegerNumber);
                accountInputField.characterLimit = 11;
            }
            else
            {
                accountInputField.SetContentType(InputField.ContentType.EmailAddress);
                accountInputField.characterLimit = 0;
            }

            if (FunnyDataStore.HasRecord)
            {
                LoginAccountRecord record = FunnyDataStore.GetFirstRecord();

                OnSelectRecordAction(record);
            }
        }

        private void CommitForm_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            OnLoginAccountAction();
        }

        private void SwitchFocus_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (accountInputField.isFocused)
            {
                if (IsPasswordStatus)
                {
                    pwdInputField.ActivateInputField();
                }
                else
                {
                    codeInputField.ActivateInputField();
                }
            }
            else
            {
                accountInputField.ActivateInputField();
            }
        }

        private void OnDestroy()
        {
            pageActions.Base.SwitchFocus.performed -= SwitchFocus_performed;
            pageActions.Base.CommitForm.performed -= CommitForm_performed;

            codeLoginButton.onClick.RemoveAllListeners();
            retrieveButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
            accountPwdButton.onClick.RemoveAllListeners();
            userProtocolButton.onClick.RemoveAllListeners();
            privacyProtocolButton.onClick.RemoveAllListeners();

            accountInputField.onValueChangedEvents.RemoveAllListeners();
            accountInputField.onClickArrowEvents.RemoveAllListeners();
            accountInputField.onClearContentEvents.RemoveAllListeners();

            pwdInputField.onClearContentEvents.RemoveAllListeners();
            pwdInputField.onValueChangedEvents.RemoveAllListeners();

            codeButton.onClick.RemoveAllListeners();
            loginButton.onClick.RemoveAllListeners();

            recordListPage.onSelectRecordEvents -= OnSelectRecordAction;
            recordListPage.onDeleteRecordEvents -= OnDeleteRecordAction;
            recordListPage.onEmptyListEvents -= OnRecordListEmptyAction;

            PCLoginView.OnInvaidTokenResultAction -= OnInvaidTokenAction;
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

        internal void EnterPwdPage()
        {
            ExitCodePage();

            IsPasswordStatus = true;

            pwdInputField.gameObject.SetActive(true);
            leftMeun.SetActive(true);

            rememberToggle.gameObject.SetActive(true);

            bottomMeun.SetActive(BridgeConfig.IsMainland);

            gameObject.SetActive(true);
        }

        internal void ExitPwdPage()
        {
            gameObject.SetActive(false);
            pwdInputField.gameObject.SetActive(false);
            leftMeun.SetActive(false);
            recordListPage.Hide();
        }

        internal void EnterCodePage()
        {
            ExitPwdPage();

            IsPasswordStatus = false;

            smsContainer.SetActive(true);
            accountPwdButton.gameObject.SetActive(true);
            rememberToggle.gameObject.SetActive(false);

            bottomMeun.SetActive(BridgeConfig.IsMainland);

            gameObject.SetActive(true);
        }

        internal void ExitCodePage()
        {
            gameObject.SetActive(false);
            smsContainer.SetActive(false);
            accountPwdButton.gameObject.SetActive(false);
            recordListPage.Hide();
        }

        #endregion

        private void OnSendSmsCodeAction()
        {
            // 发送验证码
            // 验证账号
            string account = accountInputField.text.Trim();
            // 数据格式效验逻辑
            if (!ValidateAccount(account)) { return; }

            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

            UILoginPageState pageState = IsPasswordStatus ? UILoginPageState.PwdLoginPage : UILoginPageState.CodeLoginPage;

            timerHandler.SendingStatus();

            Funny.Core.Bridge.Common.SendVerificationCode(account, CodeAction.Login, category)
                                    .Then(() =>
                                    {
                                        timerHandler.StartTimer();
                                    })
                                    .Catch((error) =>
                                    {
                                        timerHandler.ResetTimer();
                                    });
        }

        private void OnLoginAccountAction()
        {
            if (!CheckAgreement()) return;

            if (!ValidateAccount(accountInputField.text)) return;

            // 发起登录
            if (IsPasswordStatus)
            {
                if (string.IsNullOrEmpty(pwdInputField.text))
                {
                    Toast.ShowFail(Locale.LoadText("form.password.required"));
                    return;
                }

                if (pwdInputField.text.Length < 8)
                {
                    Toast.ShowFail(Locale.LoadText("form.password.length.require"));
                    return;
                }

                if (IsLoginRecord)
                {
                    LoginAccountRecord record = FunnyDataStore.GetAccountRecord(accountInputField.text);

                    PCLoginView.OnLoginWithRecordAction?.Invoke(record, rememberToggle.isOn);
                }
                else
                {
                    PCLoginView.OnLoginWithPasswordAction?.Invoke(accountInputField.text, pwdInputField.text, rememberToggle.isOn);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(codeInputField.text))
                {
                    Toast.ShowFail(Locale.LoadText("form.code.required"));
                    return;
                }

                PCLoginView.OnLoginWithCodeAction?.Invoke(accountInputField.text, codeInputField.text);
            }

        }

        private bool CheckAgreement()
        {
            if (bottomMeun.activeSelf)
            {
                if (agreementToggle.isOn == false)
                {
                    Toast.ShowFail(Locale.LoadText("form.protocol.tips"), 1f);
                }

                return agreementToggle.isOn;
            }

            return true;
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

        private void OnSelectRecordAction(LoginAccountRecord record)
        {
            accountInputField.text = record.Account;

            if (!string.IsNullOrEmpty(record.Pwd))
            {
                pwdInputField.text = record.Pwd;
                pwdInputField.HideEye(true);

                IsLoginRecord = true;
                rememberToggle.isOn = true;
                agreementToggle.isOn = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(pwdInputField.text))
                {
                    pwdInputField.text = "";
                }

                rememberToggle.isOn = false;
                agreementToggle.isOn = false;
            }

        }

        private void OnDeleteRecordAction(string account)
        {
            if (string.IsNullOrEmpty(account)) return;

            if (accountInputField.text == account)
            {
                accountInputField.text = "";
            }
        }

        private void OnCleanAccountInputFieldAction()
        {
            if (IsLoginRecord)
            {
                pwdInputField.ShowEye();
            }
        }

        private void OnAccountInputFieldValueChangedAction(string value)
        {
            if (IsLoginRecord)
            {
                if (!string.IsNullOrEmpty(pwdInputField.text))
                {
                    pwdInputField.text = "";
                    rememberToggle.isOn = false;
                    agreementToggle.isOn = false;
                }
            }
            else
            {
                LoginAccountRecord record = FunnyDataStore.GetAccountRecord(value);

                if (record is null) return;
                if (string.IsNullOrEmpty(record.Pwd))
                {
                    rememberToggle.isOn = false;
                    agreementToggle.isOn = false;
                    return;
                }

                pwdInputField.text = record.Pwd;
                pwdInputField.HideEye(true);

                IsLoginRecord = true;
                rememberToggle.isOn = true;
                agreementToggle.isOn = true;
            }
        }

        private void OnPwdInputValueChangedAction(string value)
        {
            if (!IsLoginRecord) return;

            if (!string.IsNullOrEmpty(value))
            {
                pwdInputField.text = value.Substring(value.Length - 1);
            }

            IsLoginRecord = false;

            pwdInputField.ShowEye();
        }

        private void OnRecordListEmptyAction()
        {
            accountInputField.UpdateUI();
        }

        private void OnInvaidTokenAction()
        {
            pwdInputField.text = "";
        }

    }
}