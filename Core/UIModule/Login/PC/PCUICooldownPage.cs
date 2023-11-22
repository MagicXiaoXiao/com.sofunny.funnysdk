using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUICooldownPage : MonoBehaviour
    {
        [SerializeField]
        private Text contentText;
        [SerializeField]
        private Button recallButton;
        [SerializeField]
        private Button otherButton;


        private void Awake()
        {
            recallButton.onClick.AddListener(OnRecallAccountAction);
            otherButton.onClick.AddListener(OnOtherAccountAction);
        }

        private void OnDestroy()
        {
            recallButton.onClick.RemoveAllListeners();
            otherButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {

        }

        private void OnRecallAccountAction()
        {
            PCLoginView.OnReCallDeleteAction?.Invoke();
        }

        private void OnOtherAccountAction()
        {
            PCLoginView.OnSwitchOtherAction?.Invoke();
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

