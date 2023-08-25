using System;
namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceBillboard
    {
        void FetchAnyData(ServiceCompletedHandler<bool> handler);

        void OpenBillboard();
    }
}

