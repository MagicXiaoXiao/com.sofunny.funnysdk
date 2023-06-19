using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 获取个人中心信息，PC = PersonalCenter
    /// </summary>
    internal class PCInfoRequest : RequestBase
    {
        private readonly string TokenValue;

        internal PCInfoRequest(string token)
        {
            TokenValue = token;
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/pc/info";

        internal override string Token => TokenValue;
    }
}

