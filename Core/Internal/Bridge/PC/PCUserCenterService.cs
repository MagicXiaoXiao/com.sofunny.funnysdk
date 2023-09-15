#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCUserCenterService : IBridgeServiceUserCenter
    {
        internal PCUserCenterService()
        {
        }

        public void OpenUserCenter()
        {
            Toast.ShowFail("此功能暂未开放");
            Logger.LogWarning("PC 或 Editor 暂未开发此功能");
        }
    }
}

#endif