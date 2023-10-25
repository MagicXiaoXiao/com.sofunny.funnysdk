using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class GetAccountBindInfoRequest : RequestBase
    {
        private readonly string tokenValue;

        internal GetAccountBindInfoRequest(string token)
        {
            tokenValue = token;
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/account/oauth";

        internal override string Token => tokenValue;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                { "device_category","pc" },
                { "mainland",BridgeConfig.IsMainland}
            };
        }

    }
}

