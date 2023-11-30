using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class PostRefreshTokenRequest : RequestBase
    {

        private readonly SSOToken _token;

        internal PostRefreshTokenRequest(SSOToken token)
        {
            _token = token;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/account/refresh_token";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object> {
                {"client_id", BridgeConfig.AppID},
                {"grant_type", "refresh_token"},
                {"refresh_token", _token.RefreshToken},
            };
        }

    }
}

