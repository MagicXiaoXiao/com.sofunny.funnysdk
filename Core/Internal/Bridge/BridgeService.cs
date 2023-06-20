using System;
using System.Collections;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.Internal
{

    internal partial class BridgeService
    {
        internal readonly IBridgeServiceBase Common;
        internal readonly IBridgeServiceLogin Login;

        internal BridgeService(string appID, bool isMainland)
        {
            BridgeConfig.Init(appID, isMainland);

#if UNITY_ANDROID
            Common = AndroidBridge.GetInstance();
            Login = AndroidBridge.GetInstance();
#elif UNITY_STANDALONE
            Common = PCBridge.GetInstance();
            Login = PCBridge.GetInstance();
#endif
        }

    }
}

