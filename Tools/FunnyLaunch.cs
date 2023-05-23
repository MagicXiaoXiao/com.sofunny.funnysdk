using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoFunny.Tools
{

    public static class FunnyLaunch
    {

        internal static Action completionHandler;
        internal static bool isMainland;

        internal static string scenePath = "Packages/com.sofunny.funnysdk/Scenes/FunnySDKLaunchLogo";
        internal static string sceneName = "FunnySDKLaunchLogo";

        /// <summary>
        /// 显示 SoFunny 开屏动画
        /// </summary>
        /// <param name="mainland">是否使用国内样式</param>
        /// <param name="finish"></param>
        public static void Show(bool mainland, Action finish)
        {
            completionHandler = finish;
            isMainland = mainland;
            SceneManager.sceneUnloaded += UnloadLaunchScene;
            SceneManager.LoadScene(scenePath, LoadSceneMode.Additive);
        }

        private static void UnloadLaunchScene(Scene scene)
        {
            if (scene.name == sceneName)
            {
                SceneManager.sceneUnloaded -= UnloadLaunchScene;
                CallFinish();
            }
        }

        internal static void CallFinish()
        {
            completionHandler?.Invoke();
            completionHandler = null;
        }
    }

}


