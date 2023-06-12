using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace SoFunny.FunnySDK.Internal
{
    internal class LoginWithPasswordRequest : RequestBase
    {
        private readonly string Account;
        private readonly string Password;

        internal LoginWithPasswordRequest(string account, string password)
        {
            Account = account;
            Password = password;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/account/login_by_password";

        internal override bool AppID => true;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object> {
                {"account", Account },
                {"password", Password.ToMD5() }
            };
        }

    }
}

