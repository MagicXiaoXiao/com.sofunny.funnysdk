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
            // 核心服务模块
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

        /// <summary>
        /// 登出当前账号
        /// </summary>
        public static void Logout()
        {
            Core.Service.Logout();
        }

    }
}

