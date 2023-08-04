using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class ProfileTokenRequest : RequestBase
    {
        private readonly string TokenValue;

        internal ProfileTokenRequest(string nativeToken)
        {
            TokenValue = nativeToken;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/profile/token";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"authorization_token",TokenValue },
                {"category","native_token" },
            };
        }

    }
}

