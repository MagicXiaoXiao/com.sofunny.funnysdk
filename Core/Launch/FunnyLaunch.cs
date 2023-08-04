using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    public static class FunnyLaunch
    {
        internal static string LaunchPath = "FunnySDK/Launch/FunnyLaunch";

        internal static Action completionHandler;
        internal static bool isMainland;

        private static bool start = false;
        private static GameObject launch;

        /// <summary>
        /// 显示 SoFunny 开屏动画
        /// </summary>
        /// <param name="mainland">是否使用国内样式</param>
        /// <param name="finish"></param>
        public static void Show(bool mainland, Action finish)
        {
            if (start) { return; }

            start = true;

            completionHandler = finish;
            isMainland = mainland;

            GameObject prefab = Resources.Load<GameObject>(LaunchPath);
            launch = GameObject.Instantiate(prefab);

            var controller = launch.GetComponentInChildren<FunnyLaunchController>();
            controller.Show();

        }


        internal static void CallFinish()
        {
            GameObject.Destroy(launch);

            start = false;

            completionHandler?.Invoke();
            completionHandler = null;
            launch = null;
        }
    }

}


