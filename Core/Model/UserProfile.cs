using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserProfile
    {
        /// <summary>
        /// 用户唯一编号
        /// </summary>
        [JsonProperty("user_id")]
        public string ID { get; internal set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [JsonProperty("display_name")]
        public string DispalyName { get; internal set; }

        /// <summary>
        /// 用户头像 URL 地址
        /// </summary>
        [JsonProperty("picture_url")]
        public string PictureURL { get; internal set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        [JsonProperty("status_message")]
        public string StatusMessage { get; internal set; }

        /// <summary>
        /// 是否是游客身份
        /// </summary>
        [JsonProperty("guest")]
        public bool IsGuest { get; internal set; } = false;


        [JsonProperty("private_info")] internal UserPrivateInfo PrivateInfo;

        [JsonProperty("delete_flag")] internal bool DeleteFlag;

        [JsonProperty("account")] internal string Account;

        [JsonProperty("mainland")] internal bool Mainland;
    }

}

