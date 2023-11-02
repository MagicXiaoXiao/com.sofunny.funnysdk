using System;
using UnityEngine;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal class FunnyService
    {
        private readonly BridgeService bridgeService;

        internal IFunnyAccountAPI AccountAPI;
        internal IFunnyUserCenterAPI UserCenterAPI;
        internal IFunnyBillboardAPI BillboardAPI;
        internal IFunnyFeedbackAPI FeedbackAPI;

        internal FunnyService(BridgeService bridge)
        {
            // 实例化桥接服务类
            bridgeService = bridge;
            // 实例化登录服务 API
            //FunnyLoginService login = new FunnyLoginService(
            //    config,
            //    bridgeService.Common,
            //    bridgeService.Login,
            //    bridgeService.Analysis
            //    );

            // 实例化账号服务 API
            AccountAPI = new FunnyAccountService(bridgeService);
            // 实例化用户中心服务 API
            UserCenterAPI = new FunnyUserCenterService(bridgeService.UserCenter);
            // 实例化公告服务 API
            BillboardAPI = new FunnyBillboardService(bridgeService.Billboard);
            // 实例化问题反馈服务 API
            FeedbackAPI = new FunnyFeedbackSerivce(bridgeService.Feedback);
        }

        internal void Initialize()
        {
            bridgeService.Common.Initialize();
        }

        internal void SetLanguage(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    bridgeService.Common.SetLanguage("zh");
                    break;
                default:
                    bridgeService.Common.SetLanguage("en");
                    break;
            }
        }

    }
}

