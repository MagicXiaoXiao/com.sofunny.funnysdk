﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal enum ToastStyle
    {
        Normal,
        Fail,
        Success
    }

    public static class Toast
    {

        internal static bool isLoaded = false;

        private static SDKUIToast toastUI;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/Toast");
                GameObject instance = Object.Instantiate(prefab);
                instance.name = "Toast";
                toastUI = instance.GetComponent<SDKUIToast>();
                isLoaded = true;
            }
        }

        public static void Show(string text, float duration = 2f)
        {
            Prepare();
            toastUI.Init(text, duration);
        }

        public static void ShowFail(string text, float duration = 2f)
        {
            Prepare();
            toastUI.Init(text, duration, style: ToastStyle.Fail);
        }

        public static void ShowSuccess(string text, float duration = 2f)
        {
            Prepare();
            toastUI.Init(text, duration, style: ToastStyle.Success);
        }
    }
}


