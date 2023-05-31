using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public static class LoginUIService
    {
        private static bool isLoaded = false;
        private static GameObject prefab;
        private static SDKUILoginController controller;

        internal static bool display = false;

        internal static void Load()
        {
            if (isLoaded) { return; }

            prefab = Resources.Load<GameObject>("FunnySDK/UI/LoginView/LoginController");

            isLoaded = true;
        }

        private static void Prepare()
        {
            if (!display)
            {
                GameObject instance = Object.Instantiate(prefab, UIController.Instance.UIContainer.transform);
                instance.name = "LoginController";
                controller = instance.GetComponent<SDKUILoginController>();

                display = true;
            }
        }

        public static void OpenLoginSelectView()
        {
            Prepare();
            controller.OpenPage(UILoginPageState.LoginSelectPage);
        }

        public static void OpenLoginLimitView()
        {
            Prepare();
            controller.OpenPage(UILoginPageState.LoginLimitPage);
        }

    }
}


