using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    [Serializable]
    public struct TapTapConfig
    {
        [SerializeField]
        public string clientID;
        [SerializeField]
        public string clientToken;
        [SerializeField]
        public string serverURL;
        [SerializeField]
        public bool isTapBeta;
        [SerializeField]
        public bool isBonfire;


        public TapTapConfig(string clientID, string clientToken, string serverURL, bool isBonfire, bool isTapBeta)
        {
            this.clientID = clientID;
            this.clientToken = clientToken;
            this.serverURL = serverURL;
            this.isBonfire = isBonfire;
            this.isTapBeta = isTapBeta;
        }

        /// <summary>
        /// 是否设置了 TapTap 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(clientID) && !string.IsNullOrEmpty(clientToken) && !string.IsNullOrEmpty(serverURL);
            }
        }
    }
}

