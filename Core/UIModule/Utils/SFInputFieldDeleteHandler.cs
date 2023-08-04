using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(InputField))]
    public class SFInputFieldDeleteHandler : MonoBehaviour
    {
        private InputField inputField;
        private Button closeButton;

        void Awake()
        {
            inputField = GetComponent<InputField>();
            inputField.onValueChanged.AddListener(OnInputHandler);

            closeButton = GetComponentInChildren<Button>();
            closeButton.gameObject.SetActive(false);
            closeButton.onClick.AddListener(OnDeleteInputContent);
        }

        private void OnDestroy()
        {
            inputField.onValueChanged.RemoveListener(OnInputHandler);
            closeButton.onClick.RemoveListener(OnDeleteInputContent);
        }

        private void OnDeleteInputContent()
        {
            inputField.text = "";
        }

        private void OnInputHandler(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                closeButton.gameObject.SetActive(false);
            }
            else
            {
                closeButton.gameObject.SetActive(true);
            }
        }


    }
}


