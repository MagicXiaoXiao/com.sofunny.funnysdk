using System;
using UnityEngine;
using SoFunny.FunnySDK.Internal;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK
{
    internal class FunnyService
    {
        private readonly BridgeService bridgeService;

        internal IFunnyAccountAPI AccountAPI;

        internal FunnyService(FunnySDKConfig config)
        {
            // 实例化桥接服务类
            bridgeService = new BridgeService(config.AppID, config.IsMainland);
            // 实例化登录服务 API
            FunnyLoginService login = new FunnyLoginService(
                config,
                bridgeService.Common,
                bridgeService.Login,
                bridgeService.Analysis
                );

            // 实例化账号服务 API
            AccountAPI = new FunnyAccountService(login, bridgeService);
        }

        internal void Initialize()
        {
            bridgeService.Common.Initialize();
        }

    }
}

