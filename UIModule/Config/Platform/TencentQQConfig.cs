using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    [Serializable]
    public struct TencentQQConfig
    {
        [SerializeField]
        public string appID;
        [SerializeField]
        public string universalLink;

        internal TencentQQConfig(string appID, string universalLink)
        {
            this.appID = appID;
            this.universalLink = universalLink;
        }

        /// <summary>
        /// 是否设置了 QQ 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(appID);
            }
        }

    }
}

