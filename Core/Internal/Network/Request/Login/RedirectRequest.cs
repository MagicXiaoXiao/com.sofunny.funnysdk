using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class RedirectRequest : RequestBase
    {
        private readonly string TokenValue;
        private readonly PKCE pkce;

        internal RedirectRequest(string accessToken)
        {
            TokenValue = accessToken;
            pkce = new PKCE();
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/native/redirect";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>() {
                { "access_token",TokenValue },
                { "client_id",BridgeConfig.AppID },
                { "code_challenge",pkce.CodeChallenge },
                { "code_challenge_method",pkce.CodeChallengeMethod },
                { "redirect_uri","funny3rdp.com.funny.win://authorize/" },
                { "state","123" },
            };
        }

    }
}

