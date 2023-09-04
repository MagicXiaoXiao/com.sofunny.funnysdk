using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(InputField))]
    public class SFInputFieldExtension : MonoBehaviour
    {
        [Header("基础功能项")]
        public bool enableClear = false;
        public Button clearButton;
        public bool enableRecord = false;
        public Button recordButton;

        [Header("显示隐藏输入框内容项")]
        public bool enableEye = false;
        public Button openEyeButton;
        public Button closeEyeButton;
        public InputField.ContentType contentType = InputField.ContentType.Standard;

        [Header("相关功能事件项")]
        public UnityEvent onRecordButtonClick;


        private InputField inputField;

        void Awake()
        {
            inputField = GetComponent<InputField>();
            inputField.onValueChanged.AddListener(OnInputHandler);

            SetupClearButton();
            SetupRecordButton();
            SetupPasswordEye();
        }

        void OnDestroy()
        {
            inputField.onValueChanged.RemoveListener(OnInputHandler);

            clearButton?.onClick.RemoveAllListeners();
            recordButton?.onClick.RemoveAllListeners();

            openEyeButton?.onClick.RemoveAllListeners();
            closeEyeButton?.onClick.RemoveAllListeners();
        }

        public void SetInputFieldContent(string value)
        {
            inputField.text = value;
        }

        public void UpdateMeunItem()
        {
            SetupRecordButton();
        }

        private void SetupClearButton()
        {
            clearButton?.gameObject.SetActive(false);

            if (enableClear)
            {
                clearButton?.onClick.AddListener(OnClearInputContent);
            }
        }

        private void SetupRecordButton()
        {
            recordButton?.gameObject.SetActive(enableRecord);

            if (enableRecord)
            {
                recordButton?.gameObject.SetActive(FunnyDataStore.HasRecord);
                recordButton?.onClick.AddListener(OnHistoryInputList);
            }
        }

        private void SetupPasswordEye()
        {
            if (!enableEye)
            {
                openEyeButton?.gameObject.SetActive(false);
                closeEyeButton?.gameObject.SetActive(false);
                return;
            }

            if (inputField.contentType == InputField.ContentType.Password)
            {
                openEyeButton?.gameObject.SetActive(false);
                closeEyeButton?.gameObject.SetActive(true);
            }
            else
            {
                openEyeButton?.gameObject.SetActive(true);
                closeEyeButton?.gameObject.SetActive(false);
            }

            openEyeButton?.onClick.AddListener(OnHidePasswordValue);
            closeEyeButton?.onClick.AddListener(OnShowPasswordValue);
        }

        private void OnHistoryInputList()
        {
            onRecordButtonClick?.Invoke();
        }

        private void OnClearInputContent()
        {
            inputField.text = "";
        }

        private void OnInputHandler(string value)
        {
            UpdateEyeStatue(value);
        }

        private void OnEnable()
        {
            UpdateEyeStatue(inputField.text);
        }

        private void UpdateEyeStatue(string value)
        {
            if (!enableClear) { return; }

            if (string.IsNullOrEmpty(value))
            {
                clearButton?.gameObject?.SetActive(false);
            }
            else
            {
                clearButton?.gameObject?.SetActive(true);
            }
        }


        private void OnShowPasswordValue()
        {
            openEyeButton?.gameObject.SetActive(true);
            closeEyeButton?.gameObject.SetActive(false);
            inputField.contentType = contentType;
            inputField.ForceLabelUpdate();
        }

        private void OnHidePasswordValue()
        {
            openEyeButton?.gameObject.SetActive(false);
            closeEyeButton?.gameObject.SetActive(true);
            inputField.contentType = InputField.ContentType.Password;
            inputField.ForceLabelUpdate();
        }

    }
}

