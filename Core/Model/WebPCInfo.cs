using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    /*
     {
      "account": "string",
      "amr": "string",
      "avatar": "string",
      "birthday": "string",
      "deadline": 0,
      "delete_flag": true,
      "identification_card": "string",
      "mainland": true,
      "nickname": "string",
      "pass_id": "string",
      "platform": "string",
      "realname": "string",
      "set_password": true,
      "sex": "string",
      "unblocked_at": 0,
      "update_nickname_at": 0
    }
     */

    [JsonObject(MemberSerialization.OptIn)]
    internal class WebPCInfo
    {

        [JsonProperty("account")] public string Account = "";

        //[JsonProperty("delete_flag")] public bool DeleteFlag = false;

        [JsonProperty("deadline")] public long Deadline = 0;

        [JsonProperty("unblocked_at")] public long UnblockedAt = 0;

        /// <summary>
        /// 冷静期发起时间
        /// </summary>
        public string StartDate
        {
            get
            {
                if (Deadline >= 0)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(Deadline)
                            .LocalDateTime
                            .AddDays(-16)
                            .ToString("yyyy-MM-dd");
                }
                return "";
            }
        }

        /// <summary>
        /// 冷静期截止时间
        /// </summary>
        public string DeadlineDate
        {
            get
            {
                if (Deadline >= 0)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(Deadline).LocalDateTime.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }

        /// <summary>
        /// 账号被封禁时间
        /// </summary>
        public string BanDate
        {
            get
            {
                if (UnblockedAt >= 0)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(UnblockedAt).LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return "";
            }
        }
    }
}

