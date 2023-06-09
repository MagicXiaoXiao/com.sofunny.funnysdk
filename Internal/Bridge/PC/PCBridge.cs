using System;


namespace SoFunny.FunnySDK.Internal
{
    internal class PCBridge : IBridgeServiceBase
    {
        public void Call(string method, string parameter, IServiceAsyncCallbackHandler handler = null)
        {
            switch (method)
            {
                case Method.Initialize:
                    break;
                default:
                    break;
            }
        }
    }
}

