using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 个人中心 Token 转换 PC = PersonalCenter
    /// </summary>
    internal class PCTokenRequest : RequestBase
    {
        private readonly string TokenValue;

        internal PCTokenRequest(string token)
        {
            TokenValue = token;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/pc/token";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"authorization_token", TokenValue},
                {"category", "native_token"}
            };
        }
    }
}

