using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{

    internal class SFUIInputField : MonoBehaviour
    {
        [SerializeField]
        private bool enableClean = false;
        [SerializeField]
        private bool enableDownArrow = false;
        [SerializeField]
        private bool enableEye = false;
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Text placeholderText;
        [SerializeField]
        private SFUIInputFieldMeun inputFieldMeun;
        [SerializeField]
        private InputField.ContentType contentType;
        [Space]
        public UnityEvent onClickArrowEvents = new UnityEvent();
        [Space]
        public UnityEvent onClearContentEvents = new UnityEvent();
        [Space]
        public StringUnityEvent onValueChangedEvents = new StringUnityEvent();

        internal string text { get { return inputField.text.Trim(); } set { inputField.text = value; } }
        internal string placeholder { get { return placeholderText.text.Trim(); } set { placeholderText.text = value; } }
        internal bool isFocused => inputField.isFocused;

        private void Awake()
        {
            inputFieldMeun.contentType = inputField.contentType;

            UpdateUI();

            inputFieldMeun.onArrowClick.AddListener(OnClickArrowAction);
            inputFieldMeun.onCleanContent.AddListener(() => onClearContentEvents?.Invoke());
            inputField.onValueChanged.AddListener((value) => onValueChangedEvents?.Invoke(value));
        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
            inputFieldMeun.onArrowClick.RemoveAllListeners();
            inputFieldMeun.onCleanContent.RemoveAllListeners();
            inputField.onValueChanged.RemoveAllListeners();
        }

        private void OnClickArrowAction()
        {
            onClickArrowEvents?.Invoke();
        }

        internal void ActivateInputField()
        {
            inputField.ActivateInputField();
        }

        internal void UpdateUI()
        {
            inputField.contentType = contentType;

            inputFieldMeun.SetActiveClean(enableClean);

            if (enableDownArrow)
            {
                inputFieldMeun.SetActiveArrow(FunnyDataStore.HasRecord);
            }
            else
            {
                inputFieldMeun.SetActiveArrow(false);
            }

            inputFieldMeun.SetActiveEye(enableEye);
        }

        internal void HideEye(bool pwdContent = false)
        {
            if (!enableEye) return;

            inputFieldMeun.SetActiveEye(false);

            if (pwdContent)
            {
                inputFieldMeun.ResetEye();
            }
        }

        internal void ShowEye()
        {
            if (!enableEye) return;

            inputFieldMeun.SetActiveEye(true);
        }

    }
}

