using System;
using Newtonsoft.Json;


namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Value;

        [JsonProperty("expires_in")]
        public int ExpiresIn;

        [JsonProperty("refresh_token")]
        public string RefreshToken;

        [JsonProperty("is_register")]
        public bool NewUser;

        [JsonProperty("token_type")]
        public string Type;
    }
}

