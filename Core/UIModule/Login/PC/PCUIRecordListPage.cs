using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCUIRecordListPage : MonoBehaviour
    {
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private SDKUIRecordItem cell;

        internal event Action<LoginAccountRecord> onSelectRecordEvents;
        internal event Action<string> onDeleteRecordEvents;
        internal event Action onEmptyListEvents;

        internal bool IsActive => gameObject.activeSelf;

        private void Awake()
        {
            List<LoginAccountRecord> records = FunnyDataStore.GetRecordList();

            foreach (var item in records)
            {
                SDKUIRecordItem recordCell = Instantiate(cell, content);
                recordCell.Setup(item.Account);

                recordCell.onSelectedAction = (account) =>
                {
                    if (FunnyDataStore.TryGetAccountRecord(account, out var record))
                    {
                        onSelectRecordEvents?.Invoke(record);
                    }

                    Hide();
                };

                recordCell.onRemoveAction = (deleteCell, account) =>
                {
                    FunnyDataStore.RemoveAccountRecord(account);

                    deleteCell.gameObject.SetActive(false);
                    Destroy(deleteCell.gameObject);

                    onDeleteRecordEvents?.Invoke(account);

                    if (!FunnyDataStore.HasRecord)
                    {
                        onEmptyListEvents?.Invoke();
                        Hide();
                    }
                };

                recordCell.gameObject.SetActive(true);
            }
        }

        internal void Show()
        {
            gameObject.SetActive(true);
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

