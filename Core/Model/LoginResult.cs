using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class LoginResult
    {
        [JsonProperty("bind_account")]
        public bool IsNeedBind;
        [JsonProperty("bind_account_code")]
        public string BindCode;
        [JsonProperty("is_register")]
        public bool NewUser;
    }
}

