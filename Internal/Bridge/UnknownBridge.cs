using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class UnknownBridge : IBridgeServiceBase
    {

        public void Call(string method, string parameter)
        {
            Logger.LogWarning($"未知平台实现 - {method} - {parameter}");
        }

        public void CallAsync(string method, string parameter, IServiceAsyncCallbackHandler handler)
        {
            Logger.LogWarning($"未知平台实现 - {method} - {parameter}");
        }
    }
}

