using System;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class GetPrivateInfoRequest : RequestBase
    {
        private readonly string TokenValue;

        internal GetPrivateInfoRequest(string prToken)
        {
            TokenValue = prToken;
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/profile";

        internal override string Token => TokenValue;
    }
}

