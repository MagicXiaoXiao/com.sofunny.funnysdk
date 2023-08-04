using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public static class Loader
    {
        internal static bool isLoaded = false;

        private static SDKUILoading loadingUI;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/LoadingIndicator");
                GameObject instance = Object.Instantiate(prefab);
                instance.name = "Loading";
                loadingUI = instance.GetComponent<SDKUILoading>();
                isLoaded = true;
            }
        }

        public static void ShowIndicator()
        {
            Prepare();
            loadingUI.Show();
        }

        public static void HideIndicator()
        {
            if (!isLoaded) { return; }

            loadingUI.Dismiss();
        }
    }
}


