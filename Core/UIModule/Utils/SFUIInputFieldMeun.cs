using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SFUIInputFieldMeun : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Button cleanButton;
        [SerializeField]
        private Button arrowButton;
        [SerializeField]
        private Button eyeButton;
        [SerializeField]
        private Image openImage;
        [SerializeField]
        private Image closeImage;

        private bool enableClean = false;
        private bool enableArrow = false;
        private bool enableEye = false;

        internal InputField.ContentType contentType;
        private bool showEye = false;

        internal UnityEvent onArrowClick = new UnityEvent();
        internal UnityEvent onCleanContent = new UnityEvent();

        internal void SetActiveClean(bool value)
        {
            enableClean = value;

            SetupUI();
        }

        internal void SetActiveEye(bool value)
        {
            enableEye = value;
            SetupUI();
        }

        internal void SetActiveArrow(bool value)
        {
            enableArrow = value;

            SetupUI();
        }

        internal void ResetEye()
        {
            if (showEye)
            {
                OnClickEyeButton();
            }
        }

        private void Awake()
        {
            inputField.onValueChanged.AddListener(OnInputChangeValue);
            cleanButton.onClick.AddListener(OnCleanInputField);
            arrowButton.onClick.AddListener(OnClickArrowButton);
            eyeButton.onClick.AddListener(OnClickEyeButton);
        }

        private void OnDestroy()
        {
            inputField.onValueChanged.RemoveAllListeners();
            cleanButton.onClick.RemoveAllListeners();
            arrowButton.onClick.RemoveAllListeners();
            eyeButton.onClick.RemoveAllListeners();
        }

        private void SetupUI()
        {
            cleanButton.gameObject.SetActive(false);
            eyeButton.gameObject.SetActive(false);

            OnInputChangeValue(inputField.text);

            arrowButton.gameObject.SetActive(enableArrow);

            if (enableEye)
            {
                HideInputFieldContent();
            }
            else
            {
                ShowInputFieldContent();
            }
        }

        private void ShowInputFieldContent()
        {
            showEye = true;
            closeImage.gameObject.SetActive(false);
            openImage.gameObject.SetActive(true);
            inputField.contentType = this.contentType;
            inputField.ForceLabelUpdate();
        }

        private void HideInputFieldContent()
        {
            showEye = false;
            closeImage.gameObject.SetActive(true);
            openImage.gameObject.SetActive(false);
            inputField.contentType = InputField.ContentType.Password;
            inputField.ForceLabelUpdate();
        }

        private void OnCleanInputField()
        {
            inputField.text = "";
            onCleanContent.Invoke();
        }

        private void OnClickArrowButton()
        {
            onArrowClick?.Invoke();
        }

        private void OnInputChangeValue(string value)
        {
            if (enableClean)
            {
                bool clearActive = !string.IsNullOrEmpty(value);
                cleanButton.gameObject.SetActive(clearActive);
            }

            if (enableEye)
            {
                bool showEye = !string.IsNullOrEmpty(value);
                eyeButton.gameObject.SetActive(showEye);
            }
        }

        private void OnClickEyeButton()
        {
            if (showEye)
            {
                HideInputFieldContent();
            }
            else
            {
                ShowInputFieldContent();
            }
        }

    }
}

