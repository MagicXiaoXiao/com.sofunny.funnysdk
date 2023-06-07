using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SFInputFieldPwdEyeHandler : MonoBehaviour
    {
        public InputField inputField;
        public Button openEyeButton;
        public Button closeEyeButton;

        void Awake()
        {
            openEyeButton.gameObject.SetActive(false);
            openEyeButton.onClick.AddListener(OnHidePasswordValue);

            closeEyeButton.gameObject.SetActive(true);
            closeEyeButton.onClick.AddListener(OnShowPasswordValue);

            inputField.contentType = InputField.ContentType.Password;
        }

        void OnDestroy()
        {
            openEyeButton.onClick.RemoveListener(OnHidePasswordValue);
            closeEyeButton.onClick.RemoveListener(OnShowPasswordValue);
        }

        private void OnShowPasswordValue()
        {
            openEyeButton.gameObject.SetActive(true);
            closeEyeButton.gameObject.SetActive(false);
            inputField.contentType = InputField.ContentType.Standard;
            inputField.ForceLabelUpdate();
        }

        private void OnHidePasswordValue()
        {
            openEyeButton.gameObject.SetActive(false);
            closeEyeButton.gameObject.SetActive(true);
            inputField.contentType = InputField.ContentType.Password;
            inputField.ForceLabelUpdate();
        }
    }
}
