using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUIAccountLimitPage : MonoBehaviour
    {
        [SerializeField]
        private Text contentText;
        [SerializeField]
        private Button otherButton;
        [SerializeField]
        private Button contactUsButton;


        private void Awake()
        {
            otherButton.onClick.AddListener(OnOtherAccountAction);
            contactUsButton.onClick.AddListener(OnContactUsAction);
        }

        private void OnDestroy()
        {
            otherButton.onClick.RemoveAllListeners();
            contactUsButton.onClick.RemoveAllListeners();
        }

        private void OnOtherAccountAction()
        {
            PCLoginView.OnSwitchOtherAction?.Invoke();
        }

        private void OnContactUsAction()
        {
            PCLoginView.OnClickContactUS?.Invoke();
        }

        internal void Enter(string content)
        {
            contentText.text = content;

            gameObject.SetActive(true);
        }

        internal void Exit()
        {
            gameObject.SetActive(false);
        }

    }
}