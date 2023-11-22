#if UNITY_ANDROID

using System;
using Newtonsoft.Json;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class AndroidBridge : IBridgeServiceLogin
    {

        public Promise<LimitStatus> ActivationCodeCommit(string code)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                Service.Call("ActivationCodeCommit", code, new AndroidCallBack<LimitStatus>(resolve, reject));
            });
        }

        public bool IsAuthorized => Service.Call<bool>("IsAuthorized");

        public AccessToken GetCurrentAccessToken()
        {
            string jsonToken = Service.Call<string>("GetCurrentAccessToken");

            try
            {
                return JsonConvert.DeserializeObject<AccessToken>(jsonToken);
            }
            catch (JsonException ex)
            {
                Logger.LogError("数据解析失败 - " + ex.Message);
                return null;
            }
        }

        public UserProfile GetUserProfile()
        {
            string jsonValue = Service.Call<string>("GetUserProfile");

            try
            {
                return JsonConvert.DeserializeObject<UserProfile>(jsonValue);
            }
            catch (JsonException ex)
            {
                Logger.LogError("用户信息数据解析失败 - " + ex.Message);
                return null;
            }
        }

        public Promise<UserProfile> FetchUserProfile()
        {
            return new Promise<UserProfile>((resolve, reject) =>
            {
                Service.Call("FetchUserProfile", new AndroidCallBack<UserProfile>(resolve, reject));
            });
        }

        public Promise<LoginResult> LoginWithCode(string account, string code)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Service.Call("LoginWithCode", account, code, new AndroidCallBack<LoginResult>((result) =>
                {
                    FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                    resolve(result);

                }, reject));
            });
        }

        public Promise<LoginResult> LoginWithPassword(string account, string password)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Service.Call("LoginWithPassword", account, password, new AndroidCallBack<LoginResult>((result) =>
                {
                    FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                    resolve(result);

                }, reject));
            });
        }

        public Promise<LoginResult> LoginWithProvider(LoginProvider provider)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Service.Call("LoginWithProvider", (int)provider, new AndroidCallBack<LoginResult>(resolve, reject));
            });
        }

        public void Logout()
        {
            Service.Call("Logout");
        }

        public Promise<LimitStatus> NativeVerifyLimit()
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                Service.Call("NativeVerifyLimit", new AndroidCallBack<LimitStatus>(resolve, reject));
            });
        }

        public Promise<LimitStatus> RealnameInfoCommit(string realname, string cardID)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                Service.Call("RealnameInfoCommit", realname, cardID, new AndroidCallBack<LimitStatus>(resolve, reject));
            });
        }

        public Promise RecallAccountDelete()
        {
            return new Promise((resolve, reject) =>
            {
                Service.Call("RecallAccountDelete", new AndroidCallBack(resolve, reject));
            });
        }

        public Promise<LoginResult> RegisterAccount(string account, string password, string chkCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Service.Call("RegisterAccount", account, password, chkCode, new AndroidCallBack<LoginResult>((result) =>
                {
                    FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                    resolve(result);

                }, reject));
            });
        }

        public Promise RetrievePassword(string account, string password, string chkCode)
        {
            return new Promise((resolve, reject) =>
            {
                CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
                Service.Call("RetrievePassword", account, password, chkCode, (int)category, new AndroidCallBack(resolve, reject));
            });
        }

        public Promise<WebPCInfo> GetWebPCInfo()
        {
            return new Promise<WebPCInfo>((resolve, reject) =>
            {
                Service.Call("GetWebPCInfo", new AndroidCallBack<WebPCInfo>(resolve, reject));
            });
        }

        public Promise CommitPrivateInfo(string birthday, string sex)
        {
            return new Promise((resolve, reject) =>
            {
                Service.Call("CommitPrivateInfo", birthday, sex, new AndroidCallBack(resolve, reject));
            });
        }

        public Promise<UserPrivateInfo> GetPrivateProfile()
        {
            return new Promise<UserPrivateInfo>((resolve, reject) =>
            {
                Service.Call("GetPrivateProfile", new AndroidCallBack<UserPrivateInfo>(resolve, reject));
            });
        }

    }
}

#endif