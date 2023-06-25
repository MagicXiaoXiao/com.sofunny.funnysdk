using System;
using UnityEngine;
using SoFunny.FunnySDK.Internal;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK
{
    internal class FunnyService
    {
        private readonly BridgeService bridgeService;

        internal IFunnyLoginAPI LoginAPI;

        internal FunnyService(FunnySDKConfig config)
        {
            // 实例化桥接服务类
            bridgeService = new BridgeService(config.AppID, config.IsMainland);
            // 实例化登录服务 API
            LoginAPI = new FunnyLoginService(
                config,
                bridgeService.Common,
                bridgeService.Login,
                bridgeService.Analysis
                );
        }

        internal void Initialize()
        {
            bridgeService.Common.Initialize();
        }

        /// <summary>
        /// 登出当前账号
        /// </summary>
        internal void Logout()
        {
            bridgeService.Login.Logout();
        }
    }
}

