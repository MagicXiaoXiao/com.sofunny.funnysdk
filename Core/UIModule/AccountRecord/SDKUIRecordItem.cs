using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIRecordItem : MonoBehaviour
    {
        [SerializeField]
        private Text titleLabel;
        [SerializeField]
        private Button selectButton;
        [SerializeField]
        private Button removeButton;

        public UnityAction<string> onSelectedAction;

        public UnityAction<SDKUIRecordItem, string> onRemoveAction;

        public void Setup(string title)
        {
            titleLabel.text = title;

            selectButton.onClick.AddListener(OnSelectMe);
            removeButton.onClick.AddListener(OnRemoveMe);
        }

        private void UnLoad()
        {
            selectButton.onClick.RemoveAllListeners();
            removeButton.onClick.RemoveAllListeners();

            onSelectedAction = null;
            onRemoveAction = null;

            Destroy(gameObject);
        }

        private void OnRemoveMe()
        {
            onRemoveAction?.Invoke(this, titleLabel.text);

            UnLoad();
        }

        private void OnSelectMe()
        {
            onSelectedAction?.Invoke(titleLabel.text);
        }

    }
}


