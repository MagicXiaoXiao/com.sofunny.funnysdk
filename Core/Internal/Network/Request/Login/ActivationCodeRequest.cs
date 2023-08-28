using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class ActivationCodeRequest : RequestBase
    {
        private readonly string TokenValue;
        private readonly string Code;

        internal ActivationCodeRequest(string token, string code)
        {
            TokenValue = token;
            Code = code;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/native/verify_limits";

        internal override string Token => TokenValue;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                { "activation_code",Code },
            };
        }
    }
}

