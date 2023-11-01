using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class AdditionalInfoView
    {

        internal static bool isLoaded = false;

        private static SDKUIAdditionalInfoView _view;

        internal static Action<string, string> OnCommitAction;
        internal static Action OnNextTimeAction;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/AdditionalInfoView/AdditionalInfoView");
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = "FunnyAdditionalInfoView";
                _view = instance.GetComponent<SDKUIAdditionalInfoView>();
                isLoaded = true;
            }
        }

        internal static void Open(string gender = "", string date = "")
        {
            Prepare();

            _view.SetGender(gender);
            _view.SetDateValue(date);
        }

        internal static void Close()
        {
            if (isLoaded)
            {
                _view?.HideAndClose();
                _view = null;
            }
        }

    }
}

