using System;
using UnityEngine;

using Object = UnityEngine.Object;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class UIService
    {
        private static bool IsInitialized = false;

        private static UILoginManager LoginManager;

        internal static void Initialize()
        {
            if (IsInitialized) { return; }

            // 初始化 UI 容器实例
            GameObject container = new GameObject("[ FunnyUI ]");
            Object.DontDestroyOnLoad(container);

            // 初始化登录管理模块
            LoginManager = new UILoginManager(container);

            // ... 其他模块（后续添加）

            // 标记初始化完毕
            IsInitialized = true;
        }

        /// <summary>
        /// 登录服务 UI 相关 API
        /// </summary>
        internal static IServiceLoginView Login
        {
            get
            {
                return LoginManager;
            }
        }

    }
}

