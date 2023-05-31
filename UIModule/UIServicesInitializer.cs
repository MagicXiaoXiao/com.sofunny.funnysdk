using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    static class UIServicesInitializer
    {
        internal static bool IsInitialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void EnableUIServicesInitialization()
        {
            if (IsInitialized) { return; }

            var controller = Resources.Load("FunnySDK/FunnySDKController");
            var obj = Object.Instantiate(controller);
            obj.name = "[ FunnySDK ]";
            Object.DontDestroyOnLoad(obj);

            LoginUIService.Load();

            IsInitialized = true;
        }

    }
}


