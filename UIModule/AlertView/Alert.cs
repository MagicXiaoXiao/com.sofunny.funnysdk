
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public static class Alert
    {
        internal static bool isLoaded = false;

        private static GameObject prefab;

        private static SDKUIAlert Prepare()
        {
            if (!isLoaded)
            {
                prefab = Resources.Load<GameObject>("FunnySDK/UI/AlertView");
                isLoaded = true;
            }

            GameObject instance = Object.Instantiate(prefab, UIController.Instance.UIContainer.transform);
            instance.name = "AlertView";
            return instance.GetComponent<SDKUIAlert>();
        }

        public static void Show(string title, string content, AlertActionItem cancelItem = null, AlertActionItem okItem = null)
        {
            SDKUIAlert alertUI = Prepare();

            if (cancelItem == null && okItem == null)
            {
                alertUI.Show(title, content, new AlertActionItem("好的", null));
            }
            else
            {
                alertUI.Show(title, content, cancelItem, okItem);
            }

        }

    }
}

