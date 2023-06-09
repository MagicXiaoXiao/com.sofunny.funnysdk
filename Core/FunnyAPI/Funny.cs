using System;
using System.Collections;
using System.Collections.Generic;
using SoFunny.FunnySDK.Internal;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK
{
    public static partial class Funny
    {
        internal static class Bridge
        {
            private static BridgeService bridgeService;

            internal static BridgeService Service
            {
                get
                {
                    if (bridgeService == null)
                    {
                        throw new InvalidOperationException("FunnySDK 未初始化，请先调用 Initialize 方法。");
                    }
                    return bridgeService;
                }
            }

            internal static void Init()
            {
                if (bridgeService != null) { return; }

                bridgeService = new BridgeService(ConfigService.Config.AppID, ConfigService.Config.IsMainland);
            }
        }

        /// <summary>
        /// FunnySDK 核心初始化
        /// </summary>
        public static void Initialize()
        {
            // 初始化桥接服务对象
            Bridge.Init();
            // 调用桥接初始化方法
            Bridge.Service.Call(Method.Initialize);
        }
    }
}

