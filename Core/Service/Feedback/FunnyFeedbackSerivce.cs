using System;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal class FunnyFeedbackSerivce : IFunnyFeedbackAPI
    {
        private IBridgeServiceFeedback Service;

        internal FunnyFeedbackSerivce(IBridgeServiceFeedback service)
        {
            Service = service;

            BridgeNotificationCenter.Default.AddObserver(this, "event.open.feedback", () =>
            {
                OnOpenEvents?.Invoke();
            });

            BridgeNotificationCenter.Default.AddObserver(this, "event.close.feedback", () =>
            {
                OnCloseEvents?.Invoke();
            });
        }

        public event Action OnOpenEvents;
        public event Action OnCloseEvents;

        public void Open(string id = "")
        {
            Service.OpenFeedback(id);
        }
    }
}

