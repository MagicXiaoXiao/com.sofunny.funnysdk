using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    [Serializable]
    public struct GoogleConfig
    {
        [SerializeField]
        public string idToken;

        [SerializeField]
        public string gmsGamesAppId;

        public GoogleConfig(string idToken, string gmsGamesAppId)
        {
            this.idToken = idToken;
            this.gmsGamesAppId = gmsGamesAppId;
        }
        /// <summary>
        /// 是否设置了 Google 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(idToken);
            }
        }

        public bool GmsGamesEnable
        {
            get
            {
                return !string.IsNullOrEmpty(gmsGamesAppId);
            }
        }
    }
}

