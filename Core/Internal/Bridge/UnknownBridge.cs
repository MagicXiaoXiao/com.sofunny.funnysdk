using System;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.Internal
{
    internal class UnknownBridge : IBridgeServiceBase
    {

        public void Initialize()
        {
            Logger.LogWarning($"未知平台实现");
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            Logger.LogWarning($"未知平台实现");
        }
    }
}

