#if UNITY_EDITOR || UNITY_STANDALONE

using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCUserCenterService : IBridgeServiceUserCenter
    {
        internal PCUserCenterService()
        {
        }

        public void OpenUserCenter()
        {
            Logger.LogWarning("PC 或 Editor 暂未开发此功能");
        }
    }
}

#endif