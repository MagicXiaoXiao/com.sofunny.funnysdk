using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BindInfo
    {
        [JsonProperty("oauth_items")]
        internal List<BindStatusItem> Items = new List<BindStatusItem>();

        internal bool Bounded(BindingType bindingType)
        {
            BindStatusItem item = Items.FirstOrDefault(x => x.Category == (int)bindingType);

            if (item is null)
            {
                return false;
            }

            return item.IsBind;
        }
    }

    /// <summary>
    /// 绑定状态对象
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BindStatusItem
    {
        [JsonProperty("id")]
        internal string ID { get; set; }

        [JsonProperty("category")]
        internal int Category { get; set; }

        /// <summary>
        /// 当前账号是否已绑定
        /// </summary>
        [JsonProperty("bind")]
        public bool IsBind { get; internal set; }

        /// <summary>
        /// 对应绑定渠道的昵称，未绑定则为空字符
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; internal set; }

        /// <summary>
        /// 渠道类型
        /// </summary>
        public BindingType Type
        {
            get
            {
                if (Enum.IsDefined(typeof(BindingType), Category))
                {
                    return (BindingType)Category;
                }
                else
                {
                    return BindingType.Unknown;
                }
            }
        }

    }

}

