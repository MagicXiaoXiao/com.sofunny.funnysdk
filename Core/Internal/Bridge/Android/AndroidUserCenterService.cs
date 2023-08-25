#if UNITY_ANDROID
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidUserCenterService : IBridgeServiceUserCenter
    {
        private AndroidOldService Service;

        internal AndroidUserCenterService(AndroidOldService service)
        {
            Service = service;
        }

        public void OpenUserCenter()
        {
            Service.Call("openUserCenter");
        }
    }
}

#endif