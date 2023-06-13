using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserProfile
    {
        [JsonProperty("openid")]
        public string OpenID;
        [JsonProperty("unionid")]
        public string UnionID;

        [JsonProperty("display_name")]
        public string DispalyName;

        [JsonProperty("picture_url")]
        public string PictureURL;

        [JsonProperty("amr")]
        internal string Amr;
        [JsonProperty("platform")]
        internal string Platform;
        [JsonProperty("delete_flag")]
        internal bool DeleteFlag;

        [JsonProperty("account")]
        internal string Account;
        [JsonProperty("mainland")]
        internal bool Mainland;
        [JsonProperty("correlated_name")]
        internal string CorrelatedName;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

}

