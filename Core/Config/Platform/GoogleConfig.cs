using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    [Serializable]
    public struct GoogleConfig
    {
        [SerializeField]
        public string AndroidClientID;
        [SerializeField]
        public string iOSClientID;
        [SerializeField]
        public string iOSURLScheme;

        [SerializeField]
        public string gmsGamesAppId;

        public GoogleConfig(string androidClient, string iosClient, string iosURLScheme, string gmsGamesAppId)
        {
            this.AndroidClientID = androidClient;
            this.iOSClientID = iosClient;
            this.iOSURLScheme = iosURLScheme;
            this.gmsGamesAppId = gmsGamesAppId;
        }
        /// <summary>
        /// 是否设置了 Google 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
#if UNITY_ANDROID
                return !string.IsNullOrEmpty(AndroidClientID);
#endif

#if UNITY_IOS
                return !string.IsNullOrEmpty(iOSClientID) && !string.IsNullOrEmpty(iOSURLScheme);
#endif

#if UNITY_STANDALONE
                return false;
#endif
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

