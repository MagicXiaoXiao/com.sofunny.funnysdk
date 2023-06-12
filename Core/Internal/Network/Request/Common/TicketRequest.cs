using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class TicketRequest : RequestBase
    {
        internal TicketRequest()
        {

        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/common/ticket";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>() {
                {"client_id", BridgeConfig.AppID }
            };
        }
    }
}

