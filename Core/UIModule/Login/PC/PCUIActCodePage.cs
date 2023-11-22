using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUIActCodePage : MonoBehaviour
    {
        [SerializeField]
        private SFUIInputField actCodeInputField;
        [SerializeField]
        private Button commitButton;
        [SerializeField]
        private Button orderButton;


        private void Awake()
        {
            commitButton.onClick.AddListener(OnCommitAction);
            orderButton.onClick.AddListener(OnOtherAccountAction);
        }

        private void OnDestroy()
        {
            commitButton.onClick.RemoveAllListeners();
            orderButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {

        }

        private void OnCommitAction()
        {
            string code = actCodeInputField.text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                Toast.ShowFail(Locale.LoadText("page.activeCode.title"));
                return;
            }

            PCLoginView.OnCommitActivationAction?.Invoke(code);
        }

        private void OnOtherAccountAction()
        {
            PCLoginView.OnSwitchOtherAction?.Invoke();
        }

        internal void Enter()
        {
            gameObject.SetActive(true);
        }

        internal void Exit()
        {
            gameObject.SetActive(false);
            actCodeInputField.text = "";
        }

    }
}