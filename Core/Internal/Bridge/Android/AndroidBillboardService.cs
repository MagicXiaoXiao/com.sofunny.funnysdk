#if UNITY_ANDROID

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidBillboardService : IBridgeServiceBillboard
    {
        private AndroidOldService Service;
        private long CallBackTaskId = 0;

        internal AndroidBillboardService(AndroidOldService service)
        {
            Service = service;
        }

        public void FetchAnyData(ServiceCompletedHandler<bool> handler)
        {
            CallBackTaskId ++;
            Logger.Log($"callBackTaskId: " + CallBackTaskId);
            var parameter = NativeParameter.Builder().TaskID("CallBackTaskId.ToString()");
            Service.Call("anyBillMessage", parameter, handler);
        }

        public void OpenBillboard()
        {
            Service.Call("openBillboard");
        }
    }
}

#endif
