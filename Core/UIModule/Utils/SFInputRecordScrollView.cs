using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.UIModule
{
    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    public class SFInputRecordScrollView : MonoBehaviour, IPointerClickHandler
    {
        public GameObject scrollContent;
        public GameObject itemPerfab;

        public StringEvent onSelectRecord;
        public UnityEvent onRemoveItem;

        private List<SDKUIRecordItem> contentList = new List<SDKUIRecordItem>();

        private void Awake()
        {

        }

        public void Show()
        {
            var recordList = FunnyDataStore.GetAccountHistory();

            foreach (var item in recordList)
            {
                var perfab = Instantiate(itemPerfab);
                var recordItem = perfab.GetComponent<SDKUIRecordItem>();

                recordItem.Setup(item);

                recordItem.onRemoveAction = (target, value) =>
                {
                    FunnyDataStore.RemoveAccount(value);
                    contentList.Remove(target);

                    if (contentList.Count <= 0)
                    {
                        Hide();
                    }

                    onRemoveItem?.Invoke();
                };

                recordItem.onSelectedAction = (value) =>
                {
                    onSelectRecord?.Invoke(value);
                    Hide();
                };

                perfab.transform.SetParent(scrollContent.transform);
                perfab.transform.localScale = Vector3.one;
                perfab.SetActive(true);

                contentList.Add(recordItem);
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            foreach (var item in contentList)
            {
                Destroy(item.gameObject);
            }

            contentList.Clear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Hide();
        }

    }
}

