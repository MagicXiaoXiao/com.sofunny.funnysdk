using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(InputField))]
    public class SFInputFieldPwdEyeHandler : MonoBehaviour
    {
        private InputField inputField;
        private Button openEyeButton;
        private Button closeEyeButton;

        void Awake()
        {
            inputField = GetComponent<InputField>();

            var buttons = GetComponentsInChildren<Button>(true);

            foreach (var button in buttons)
            {
                if (button.name == "CloseEye")
                {
                    closeEyeButton = button;
                }
                else if (button.name == "OpenEye")
                {
                    openEyeButton = button;
                }
            }

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
            inputField.Select();
        }

        private void OnHidePasswordValue()
        {
            openEyeButton.gameObject.SetActive(false);
            closeEyeButton.gameObject.SetActive(true);
            inputField.contentType = InputField.ContentType.Password;
            inputField.Select();
        }
    }
}
