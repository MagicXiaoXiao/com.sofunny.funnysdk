#if UNITY_ANDROID
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidBillboardService : IBridgeServiceBillboard
    {
        private AndroidOldService Service;

        internal AndroidBillboardService(AndroidOldService service)
        {
            Service = service;
        }

        public void FetchAnyData(ServiceCompletedHandler<bool> handler)
        {
            var parameter = NativeParameter.Builder().TaskID("000");
            Service.Call("anyBillMessage", parameter, handler);
        }

        public void OpenBillboard()
        {
            Service.Call("openBillboard");
        }
    }
}

#endif
