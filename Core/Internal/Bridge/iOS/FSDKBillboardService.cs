#if UNITY_IOS

using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKBillboardService : IBridgeServiceBillboard
    {
        internal FSDKBillboardService()
        {
        }

        public void FetchAnyData(ServiceCompletedHandler<bool> handler)
        {
            FSDKCallAndBack.Builder("FetchBillboardAnyData")
                           .AddCallbackHandler((success, value) =>
                           {
                               IosHelper.HandlerServiceCallback(success, value, handler);
                           })
                           .Invoke();
        }

        public void OpenBillboard()
        {
            FSDKCall.Builder("OpenBillboard").Invoke();
        }
    }
}

#endif
