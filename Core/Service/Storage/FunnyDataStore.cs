using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    internal static class FunnyDataStore
    {
        private const string FunnyAccessTokenKey = "com.funnysdk.datastore.token";

        internal static bool HasToken
        {
            get
            {
                return PlayerPrefs.HasKey(FunnyAccessTokenKey);
            }
        }

        internal static AccessToken GetCurrentToken()
        {
            string tokenJson = PlayerPrefs.GetString(FunnyAccessTokenKey);

            if (string.IsNullOrEmpty(tokenJson)) { return null; }

            try
            {
                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(tokenJson);
                return accessToken;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        internal static void UpdateToken(AccessToken accessToken)
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // 移动端平台不做储存
                return;
            }

            string tokenJson = JsonConvert.SerializeObject(accessToken);
            PlayerPrefs.SetString(FunnyAccessTokenKey, tokenJson);
            PlayerPrefs.Save();
        }

        internal static void DeleteToken()
        {
            PlayerPrefs.DeleteKey(FunnyAccessTokenKey);
        }
    }
}

