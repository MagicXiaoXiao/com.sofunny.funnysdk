﻿using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 提交隐私信息
    /// </summary>
    internal class PutPrivateInfoRequest : RequestBase
    {

        private readonly string TokenValue;
        private readonly string Sex;
        private readonly string Birthday;

        internal PutPrivateInfoRequest(string prToken, string sex, string birthday)
        {
            TokenValue = prToken;
            Sex = sex;
            Birthday = birthday;
        }

        internal override HttpMethod Method => HttpMethod.Put;

        internal override string Path => "/profile";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"birthday", Birthday},
                {"sex", Sex},
            };
        }

        internal override string Token => TokenValue;
    }
}

