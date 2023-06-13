using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class NativeTokenRequest : RequestBase
    {
        private readonly string Code;
        private readonly string CodeVerifier;

        internal NativeTokenRequest(string code, PKCE pkce)
        {
            Code = code;
            CodeVerifier = pkce.CodeVerifier;
        }

        internal override HttpMethod Method => HttpMethod.Post;

        internal override string Path => "/native/token";

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"authorization_code",Code },
                {"client_id",BridgeConfig.AppID },
                {"code_verifier",CodeVerifier },
                {"grant_type","authorization_code" },
            };
        }
    }
}

