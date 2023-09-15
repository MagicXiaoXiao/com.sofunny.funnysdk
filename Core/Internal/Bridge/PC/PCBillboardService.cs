#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCBillboardService : IBridgeServiceBillboard
    {
        internal PCBillboardService()
        {

        }

        public void FetchAnyData(ServiceCompletedHandler<bool> handler)
        {
            Logger.LogError("PC 或 Editor 暂未开发此功能");
            handler?.Invoke(false, null);
        }

        public void OpenBillboard()
        {
            Toast.ShowFail("此功能暂未开放");
            Logger.LogError("PC 或 Editor 暂未开发此功能");
        }
    }
}

#endif
