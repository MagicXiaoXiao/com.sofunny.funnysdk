using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class BindView
    {
        internal static bool isLoaded = false;

        private static SDKUIBindView _view;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/BindView/BindView");
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = "FunnyBindView";
                _view = instance.GetComponent<SDKUIBindView>();
                isLoaded = true;
            }
        }

        internal static Action<string, string, string> OnCommitAction;
        internal static Action OnCancelAction;

        internal static void Open()
        {
            Prepare();
        }

        internal static void Close()
        {
            _view?.HideAndClose();
            _view = null;
        }

    }
}

