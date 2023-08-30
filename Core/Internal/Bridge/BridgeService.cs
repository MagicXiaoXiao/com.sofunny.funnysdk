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

        /// <summary>
        /// 用户中心服务功能
        /// </summary>
        internal readonly IBridgeServiceUserCenter UserCenter;

        /// <summary>
        /// 公告服务功能
        /// </summary>
        internal readonly IBridgeServiceBillboard Billboard;

        /// <summary>
        /// 问题反馈服务功能
        /// </summary>
        internal readonly IBridgeServiceFeedback Feedback;

        /// <summary>
        /// 协议服务功能
        /// </summary>
        internal readonly IFunnyAgreementAPI Agreement;


        internal BridgeService(FunnySDKConfig config)
        {
            BridgeConfig.Init(config.AppID, config.IsMainland);

#if UNITY_STANDALONE || UNITY_EDITOR
            Common = PCBridge.GetInstance();
            Login = PCBridge.GetInstance();
            Analysis = new PCAnalysisService();
            UserCenter = new PCUserCenterService();
            Billboard = new PCBillboardService();
            Feedback = new PCFeedbackService();
            Agreement = new FunnyAgreementService(Common);
#elif UNITY_ANDROID
            Common = AndroidBridge.GetInstance();
            Login = AndroidBridge.GetInstance();
            Analysis = AndroidBridge.GetInstance();
            UserCenter = new AndroidUserCenterService(AndroidBridge.GetInstance().OldService);
            Billboard = new AndroidBillboardService(AndroidBridge.GetInstance().OldService);
            Feedback = new AndroidFeedbackService(AndroidBridge.GetInstance().OldService);
            Agreement = new FunnyAgreementService(Common);
#elif UNITY_IOS
            Common = new FSDKCommon();
            Login = new FSDKLoginService();
            Analysis = new FSDKAnalysisService();
            UserCenter = new FSDKUserCenterService();
            Billboard = new FSDKBillboardService();
            Feedback = new FSDKFeedbackService();
            Agreement = new FunnyAgreementService(Common);
#endif
        }

    }
}

