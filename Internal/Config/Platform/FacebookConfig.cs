using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    [Serializable]
    public struct FacebookConfig
    {
        [SerializeField]
        public string appID;

        [SerializeField]
        public string clientToken;

        [SerializeField]
        public bool trackEnable;

        public FacebookConfig(string appID, string clientToken, bool trackEnable = false)
        {
            this.appID = appID;
            this.clientToken = clientToken;
            this.trackEnable = trackEnable;
        }

        /// <summary>
        /// 是否设置了 Facebook 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(appID) && !string.IsNullOrEmpty(clientToken);
            }
        }
    }
}

