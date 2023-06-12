using System;
using UnityEngine;

using Object = UnityEngine.Object;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class UIService
    {
        internal static bool IsInitialized = false;

        private static UILoginManager loginManager;

        internal static void Initialize()
        {
            if (IsInitialized) { return; }

            var controller = Resources.Load("FunnySDK/FunnySDKController");
            var obj = Object.Instantiate(controller);
            obj.name = "[ FunnySDK ]";
            Object.DontDestroyOnLoad(obj);

            loginManager = new UILoginManager();

            IsInitialized = true;
        }

        internal static IServiceLoginView Login
        {
            get
            {
                return loginManager;
            }
        }

    }
}

