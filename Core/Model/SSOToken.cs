using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SSOToken
    {
        [JsonProperty("access_token")]
        public string Value;

        [JsonProperty("refresh_token")]
        public string RefreshToken;

        [JsonProperty("expires_in")]
        public int ExpiresIn;
    }
}

