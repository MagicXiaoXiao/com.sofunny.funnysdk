using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class SendCodeRequest : RequestBase
    {
        internal string Account;
        internal int CodeAction;
        internal int CodeCategory;

        internal SendCodeRequest(string account, CodeAction codeAction, CodeCategory codeCategory)
        {
            Account = account;
            CodeAction = (int)codeAction;
            CodeCategory = (int)codeCategory;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/common/create_code";
        internal override bool AppID => true;

        internal override string Token
        {
            get
            {
                if (FunnyDataStore.HasToken)
                {
                    return FunnyDataStore.Current.Value;
                }

                return "";
            }
        }

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object> {
                {"account", Account},
                {"code_action", CodeAction},
                {"code_category", CodeCategory},
            };
        }
    }
}

