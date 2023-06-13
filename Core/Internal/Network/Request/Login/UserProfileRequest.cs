using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class UserProfileRequest : RequestBase
    {
        private readonly string TokenValue;

        internal UserProfileRequest(string tokenValue)
        {
            TokenValue = tokenValue;
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/native/profile";

        internal override string Token => TokenValue;

        //internal override Dictionary<string, object> Parameters()
        //{
        //    return new Dictionary<string, object>()
        //    {
        //        {"access_token", TokenValue }
        //    };
        //}
    }
}

