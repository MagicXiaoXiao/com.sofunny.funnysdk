using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoFunny.FunnySDK.UIModule
{
    internal class UIAdditionalInfoManager : IServiceAdditionalInfoView
    {
        private readonly GameObject Container;
        private GameObject ViewPrefab;
        private SDKUIAdditionalInfoView View;
        private bool Display = false;

        internal UIAdditionalInfoManager(GameObject container)
        {
            Container = container;
        }

        private void Prepare()
        {
            if (ViewPrefab is null)
            {
                ViewPrefab = Resources.Load<GameObject>("FunnySDK/UI/AdditionalInfoView/AdditionalInfoView");
            }

            GameObject instance = Object.Instantiate(ViewPrefab, Container.transform);
            instance.name = "AdditionalInfoView";
            View = instance.GetComponent<SDKUIAdditionalInfoView>();
        }

        public void Open(IAdditionalInfoDelegate infoDelegate, string gender = "", string date = "")
        {
            if (Display) { return; }

            Prepare();

            View.InfoDelegate = infoDelegate;
            View.SetGender(gender);
            View.SetDateValue(date);

            Display = true;
        }

        public void Close()
        {
            if (!Display) { return; }

            Object.Destroy(View.gameObject);
            View = null;

            Display = false;
        }

        public void SetDateValue(string date)
        {
            if (!Display) { return; }
            if (string.IsNullOrEmpty(date)) { return; }

            View.SetDateValue(date);

        }

    }
}

