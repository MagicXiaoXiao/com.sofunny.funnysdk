#if UNITY_IOS
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKUserCenterService : IBridgeServiceUserCenter
    {
        internal FSDKUserCenterService()
        {

        }

        public void OpenUserCenter()
        {
            FSDKCall.Builder("OpenUserCenter").Invoke();
        }
    }
}

#endif