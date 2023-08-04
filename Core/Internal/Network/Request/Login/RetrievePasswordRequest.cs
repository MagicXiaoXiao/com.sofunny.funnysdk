using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class RetrievePasswordRequest : RequestBase
    {
        private readonly string Account;
        private readonly string NewPassword;
        private readonly string Code;
        private readonly int Category;

        internal RetrievePasswordRequest(string account, string newPwd, string code, CodeCategory codeCategory)
        {
            Account = account;
            NewPassword = newPwd;
            Code = code;
            Category = (int)codeCategory;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/account/change_password";

        internal override bool AppID => true;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                { "account",Account },
                { "chk_code",Code },
                { "code_category", Category },
                { "password", NewPassword.ToMD5()},
            };
        }
    }
}

