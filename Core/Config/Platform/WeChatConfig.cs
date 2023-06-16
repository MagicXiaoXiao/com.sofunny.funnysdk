using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    [Serializable]
    public struct WeChatConfig
    {
        [SerializeField]
        public string appID;
        [SerializeField]
        public string universalLink;

        public WeChatConfig(string appID, string universalLink)
        {
            this.appID = appID;
            this.universalLink = universalLink;
        }

        /// <summary>
        /// 是否设置了 WeChat 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(appID) && !string.IsNullOrEmpty(universalLink);
            }
        }

    }
}

