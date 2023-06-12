using System;
using System.Collections;
using System.Collections.Generic;
using SoFunny.FunnySDK.UIModule;

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
                Logger.Log("初始化调用 Initialize 2");
                if (internalService != null) { return; }

                internalService = new FunnyService(ConfigService.Config);

                internalService.Initialize();
            }
        }

        /// <summary>
        /// FunnySDK 核心初始化
        /// </summary>
        public static void Initialize()
        {
            Logger.Log("初始化调用 Initialize 1");
            // 初始化桥接服务对象
            Core.Initialize();

            // UI 服务模块
            UIService.Initialize();
        }

    }

    public partial class Funny
    {
        /// <summary>
        /// 登录服务
        /// </summary>
        public static IFunnyLoginAPI Login
        {
            get
            {
                return Core.Service.LoginAPI;
            }
        }
    }
}

