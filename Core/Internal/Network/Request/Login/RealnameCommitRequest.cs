using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class RealnameCommitRequest : RequestBase
    {
        private readonly string TokenValue;
        private readonly string Name;
        private readonly string CardID;

        internal RealnameCommitRequest(string token, string name, string cardID)
        {
            TokenValue = token;
            Name = name;
            CardID = cardID;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/native/verify_limits";

        internal override string Token => TokenValue;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"card_name", Name },
                {"card_id", CardID },
            };
        }
    }
}

