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
        internal string Ticket;

        internal SendCodeRequest(string account, CodeAction codeAction, CodeCategory codeCategory, string ticket)
        {
            Account = account;
            CodeAction = (int)codeAction;
            CodeCategory = (int)codeCategory;
            Ticket = ticket;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/common/create_code";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object> {
                {"account", Account},
                {"code_action", CodeAction},
                {"code_category", CodeCategory},
                {"ticket", Ticket},
            };
        }
    }
}

