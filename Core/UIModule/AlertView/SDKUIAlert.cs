using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUIAlert : MonoBehaviour
    {
        public Text titleLabel;
        public Text contentLabel;
        public Button cancelButton;
        public Button okButton;

        private Action cancelAction;
        private Action okAction;

        private void Awake()
        {
            gameObject.SetActive(false);
            cancelButton.onClick.AddListener(OnCancelHandler);
            okButton.onClick.AddListener(OnOKHandler);
        }

        private void OnDestroy()
        {
            cancelButton.onClick.RemoveListener(OnCancelHandler);
            okButton.onClick.RemoveListener(OnOKHandler);

            this.cancelAction = null;
            this.okAction = null;
        }

        private void CloseView()
        {
            gameObject.SetActive(false);
        }

        public void Show(string title, string content, AlertActionItem cancelItem = null, AlertActionItem okItem = null)
        {
            titleLabel.text = title;
            contentLabel.text = content;

            if (cancelItem != null)
            {
                this.cancelAction = cancelItem.clickAction;
                cancelButton.GetComponentInChildren<Text>().text = cancelItem.label;
                okButton.gameObject.SetActive(true);
            }
            else
            {
                okButton.gameObject.SetActive(false);
            }

            if (okItem != null)
            {
                this.okAction = okItem.clickAction;
                okButton.GetComponentInChildren<Text>().text = okItem.label;
                okButton.gameObject.SetActive(true);
            }
            else
            {
                okButton.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }

        private void OnCancelHandler()
        {
            CloseView();
            cancelAction?.Invoke();
            Destroy(gameObject);
        }

        private void OnOKHandler()
        {
            CloseView();
            okAction?.Invoke();

            Destroy(gameObject);
        }
    }
}


