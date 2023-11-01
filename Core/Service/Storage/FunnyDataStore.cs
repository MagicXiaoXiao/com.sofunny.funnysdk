using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal static partial class FunnyDataStore
    {
        private static readonly string FunnyAccessTokenKey = $"com.funnysdk.datastore.token{BridgeConfig.AppID}";

        private static SSOToken _ssoToken;

        internal static bool HasToken
        {
            get
            {
                return PlayerPrefs.HasKey(FunnyAccessTokenKey);
            }
        }

        internal static SSOToken GetCurrentToken()
        {
            if (_ssoToken != null)
            {
                return _ssoToken;
            }

            string tokenJson = PlayerPrefs.GetString(FunnyAccessTokenKey);

            if (string.IsNullOrEmpty(tokenJson)) { return null; }

            try
            {
                SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(tokenJson);
                return ssoToken;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        internal static void UpdateToken(SSOToken ssoToken)
        {
            string tokenJson = JsonConvert.SerializeObject(ssoToken);

            _ssoToken = ssoToken;

            PlayerPrefs.SetString(FunnyAccessTokenKey, tokenJson);
            PlayerPrefs.Save();
        }

        internal static void DeleteToken()
        {
            _ssoToken = null;
            PlayerPrefs.DeleteKey(FunnyAccessTokenKey);
        }
    }
}

