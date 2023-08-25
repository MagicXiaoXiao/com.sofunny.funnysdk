using System;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal class FunnyUserCenterService : IFunnyUserCenterAPI
    {
        private readonly IBridgeServiceUserCenter Service;

        internal FunnyUserCenterService(IBridgeServiceUserCenter bridgeService)
        {
            Service = bridgeService;

            BridgeNotificationCenter.Default.AddObserver(this, "event.open.userCenter", OnUserCenterOpen);
            BridgeNotificationCenter.Default.AddObserver(this, "event.close.userCenter", OnUserCenterClose);
        }

        public event Action OnOpenEvents;
        public event Action OnCloseEvents;

        public void Open()
        {
            Service.OpenUserCenter();
        }

        private void OnUserCenterOpen()
        {
            OnOpenEvents?.Invoke();
        }

        private void OnUserCenterClose()
        {
            OnCloseEvents?.Invoke();
        }

    }
}

