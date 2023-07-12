using System;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class NativeVerifyLimitRequest : RequestBase
    {
        private readonly string TokenValue;

        internal NativeVerifyLimitRequest(string token)
        {
            TokenValue = token;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/native/verify_limits";

        internal override string Token => TokenValue;
    }
}

