using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SFInputFieldSyncValueChangeHandler : MonoBehaviour
    {
        public List<InputField> inputFields;

        private void Awake()
        {
            foreach (var inputField in inputFields)
            {
                inputField.onValueChanged.AddListener(OnInputChangeValue);
            }
        }

        private void OnInputChangeValue(string value)
        {
            foreach (var inputField in inputFields)
            {
                inputField.text = value;
            }
        }

        private void OnDestroy()
        {
            foreach (var inputField in inputFields)
            {
                inputField.onValueChanged.RemoveAllListeners();
            }
        }
    }
}


