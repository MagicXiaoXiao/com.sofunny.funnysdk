using System;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class RecallAccountDeleteRequest : RequestBase
    {
        private readonly string TokenValue;

        internal RecallAccountDeleteRequest(string token)
        {
            TokenValue = token;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/native/recall_delete";

        internal override string Token => TokenValue;
    }
}

