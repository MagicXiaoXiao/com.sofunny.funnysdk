using System;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal class FunnyBillboardService : IFunnyBillboardAPI
    {
        private readonly IBridgeServiceBillboard Service;

        internal FunnyBillboardService(IBridgeServiceBillboard service)
        {
            Service = service;

            BridgeNotificationCenter.Default.AddObserver(this, "event.open.billboard", () =>
            {
                OnOpenEvents?.Invoke();
            });

            BridgeNotificationCenter.Default.AddObserver(this, "event.close.billboard", () =>
            {
                OnCloseEvents?.Invoke();
            });
        }

        public event Action OnOpenEvents;
        public event Action OnCloseEvents;


        public void Open()
        {
            Service.OpenBillboard();
        }

        public void FetchAny(Action<bool> action)
        {
            Service.FetchAnyData((has, error) =>
            {
                if (error is null)
                {
                    action?.Invoke(has);
                }
                else
                {
                    Logger.LogError($"FunnyBillboard - HasAny error. - {error.Code}|>{error.Message}");
                    action?.Invoke(false);
                }
            });
        }
    }
}

