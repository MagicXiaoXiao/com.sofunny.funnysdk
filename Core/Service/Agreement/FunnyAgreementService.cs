using System;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal class FunnyAgreementService : IFunnyAgreementAPI
    {

        private readonly IBridgeServiceBase Service;

        internal FunnyAgreementService(IBridgeServiceBase service)
        {
            Service = service;

            BridgeNotificationCenter.Default.AddObserver(this, "event.open.protocol", () =>
            {
                OnComfirmProtocolEvent?.Invoke();
            });
        }

        public event Action OnComfirmProtocolEvent;

        public void Open()
        {
            Service.OpenAgreenment();
        }
    }
}

