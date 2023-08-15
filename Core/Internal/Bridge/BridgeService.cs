using System;
using System.Collections;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.Internal
{

    internal partial class BridgeService
    {
        /// <summary>
        /// 基础服务功能
        /// </summary>
        internal readonly IBridgeServiceBase Common;

        /// <summary>
        /// 登录服务功能
        /// </summary>
        internal readonly IBridgeServiceLogin Login;

        /// <summary>
        /// 数据收集服务功能
        /// </summary>
        internal readonly IBridgeServiceTrack Analysis;

        internal BridgeService(string appID, bool isMainland)
        {
            BridgeConfig.Init(appID, isMainland);

#if UNITY_ANDROID && !UNITY_EDITOR
            Common = AndroidBridge.GetInstance();
            Login = AndroidBridge.GetInstance();
            Analysis = AndroidBridge.GetInstance();
#elif UNITY_IOS && !UNITY_EDITOR
            Common = new FSDKCommon();
            Login = new FSDKLoginService();
            Analysis = new FSDKAnalysisService();
#elif UNITY_STANDALONE || UNITY_EDITOR
            Common = PCBridge.GetInstance();
            Login = PCBridge.GetInstance();
            Analysis = new PCAnalysisService();
#endif
        }

    }
}

