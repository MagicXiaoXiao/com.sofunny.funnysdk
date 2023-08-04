using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class LoginWithCodeRequest : RequestBase
    {
        private readonly string Account;
        private readonly string Code;

        internal LoginWithCodeRequest(string account, string code)
        {
            Account = account;
            Code = code;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/account/login_by_chk_code";

        internal override bool AppID => true;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>() {
                {"account", Account },
                {"chk_code", Code },
            };
        }
    }
}

