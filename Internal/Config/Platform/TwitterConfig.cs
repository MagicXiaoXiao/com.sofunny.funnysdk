using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    [Serializable]
    public struct TwitterConfig
    {
        [SerializeField]
        public string consumerKey;

        [SerializeField]
        public string consumerSecret;

        public TwitterConfig(string consumerKey, string consumerSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        /// <summary>
        /// 是否设置了 Twitter 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(consumerKey) && !string.IsNullOrEmpty(consumerSecret);
            }
        }

    }
}

