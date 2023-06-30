using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserPrivateInfo
    {
        [JsonProperty("sex")]
        public string Sex;

        [JsonProperty("birthday")]
        public string Birthday;

        /// <summary>
        /// 信息是否填写完整
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(Birthday) && !string.IsNullOrEmpty(Sex) && Sex != "SECRET";
            }
        }
    }
}

