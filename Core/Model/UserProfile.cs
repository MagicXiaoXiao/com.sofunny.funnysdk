using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserProfile
    {

        [JsonProperty("openid")] public string OpenID;

        [JsonProperty("unionid")] public string UnionID;

        /// <summary>
        /// 用户定义名称
        /// </summary>
        [JsonProperty("display_name")] public string DispalyName;
        /// <summary>
        /// 用户头像 URL
        /// </summary>
        [JsonProperty("picture_url")] public string PictureURL;
        /// <summary>
        /// 状态消息
        /// </summary>
        [JsonProperty("status_message")] public string StatusMessage;

        [JsonProperty("private_info")] internal UserPrivateInfo PrivateInfo;

        [JsonProperty("amr")] internal string Amr;

        [JsonProperty("platform")] internal string Platform;

        [JsonProperty("delete_flag")] internal bool DeleteFlag;


        [JsonProperty("account")] internal string Account;

        [JsonProperty("mainland")] internal bool Mainland;

        [JsonProperty("correlated_name")] internal string CorrelatedName;


    }

}

