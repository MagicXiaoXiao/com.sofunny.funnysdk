using System;
using Newtonsoft.Json;


namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AccessToken
    {
        [JsonProperty("oauth_access_token")]
        public string Value;

        [JsonProperty("expires_in")]
        public int ExpiresIn;
    }
}

