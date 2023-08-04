using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserPrivateInfo
    {
        /// <summary>
        /// 性别 MALE 或 FEMALE
        /// </summary>
        [JsonProperty("sex")] public string Gender;

        /// <summary>
        /// 出生日期 yyyy-MM-dd
        /// </summary>
        [JsonProperty("birthday")] public string Birthday;

        /// <summary>
        /// 信息是否填写完整
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(Birthday) && !string.IsNullOrEmpty(Gender) && Gender != "SECRET";
            }
        }
    }
}

