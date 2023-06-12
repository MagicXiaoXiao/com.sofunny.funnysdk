using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class RegisterAccountRequest : RequestBase
    {
        internal string Account;
        internal string Password;
        internal string Code;

        internal RegisterAccountRequest(string account, string password, string code)
        {
            Account = account;
            Password = password;
            Code = code;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/account/signup";

        internal override bool AppID => true;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"account", Account },
                {"password", Password.ToMD5() },
                {"chk_code", Code },
            };
        }
    }
}

