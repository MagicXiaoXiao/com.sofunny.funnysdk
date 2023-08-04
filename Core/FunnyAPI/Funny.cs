using System;
using System.Collections;
using System.Collections.Generic;
using SoFunny.FunnySDK.UIModule;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    public static partial class Funny
    {
        internal static class Core
        {
            private static FunnyService internalService;

            internal static FunnyService Service
            {
                get
                {
                    if (internalService == null)
                    {
                        throw new InvalidOperationException("FunnySDK 未初始化，请先调用 Initialize 方法。");
                    }
                    return internalService;
                }
            }

            internal static void Initialize()
            {
                if (internalService != null) { return; }

                internalService = new FunnyService(ConfigService.Config);

                internalService.Initialize();
                internalService.SetLanguage(Locale.PlayerLanguage);
            }
        }

        private static bool SdkSetup = false;

        /// <summary>
        /// FunnySDK 核心初始化
        /// </summary>
        public static void Initialize()
        {
            if (SdkSetup) { return; }

            // 核心服务模块
            Core.Initialize();

            // UI 服务模块
            UIService.Initialize();

            // 初始化结束标记
            SdkSetup = true;
        }

        /// <summary>
        /// 设置 SDK 语言（暂时只支持简体中文与英文）
        /// </summary>
        /// <param name="language"></param>
        public static void SetLanguage(SystemLanguage language)
        {
            if (SdkSetup)
            {
                Core.Service.SetLanguage(language);
            }

            Locale.SetCurrentLanguage(language);
        }

    }

    public partial class Funny
    {
        /// <summary>
        /// 账号相关服务
        /// </summary>
        public static IFunnyAccountAPI Account => Core.Service.AccountAPI;

    }
}

