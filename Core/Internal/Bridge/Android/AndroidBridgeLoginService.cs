#if UNITY_ANDROID

using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class AndroidBridge : IBridgeServiceLogin
    {
        public void ActivationCodeCommit(string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("ActivationCodeCommit", code, new AndroidCallBack<LimitStatus>(handler));
        }
        // TODO: 新增 - 返回是否已进行过登录授权
        public bool IsAuthorized => Service.Call<bool>("IsAuthorized");

        public AccessToken GetCurrentAccessToken()
        {
            // TODO: 调整 - 该 Token 值为用户 Token，（oauth_access_token）
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
            // TODO: 新增 - 直接获取用户信息，不走服务器请求
            // 理想情况调整为 Service.Call<UserProfile>("GetUserProfile");
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

        public void FetchUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            // TODO: 新增 - 从服务器请求获取当前用户信息
            Service.Call("FetchUserProfile", new AndroidCallBack<UserProfile>(handler));
        }

        public void GetUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            // TODO: 移除 - 后续版本将会移除 GetUserProfile 方法。当前使用 FetchUserProfile
            FetchUserProfile(handler);
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<LoginResult> handler)
        {
            // TODO: 调整 - 将原为 AccessToken 类型调整为 LoginResult
            Service.Call("LoginWithCode", account, code, new AndroidCallBack<LoginResult>(handler));
        }

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<LoginResult> handler)
        {
            // TODO: 调整 - 将原为 AccessToken 类型调整为 LoginResult
            Service.Call("LoginWithPassword", account, password, new AndroidCallBack<LoginResult>(handler));
        }

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<LoginResult> handler)
        {
            // TODO: 调整 - 将原为 AccessToken 类型调整为 LoginResult
            Service.Call("LoginWithProvider", ((int)provider), new AndroidCallBack<LoginResult>(handler));
        }

        public void Logout()
        {
            Service.Call("Logout");
        }

        public void NativeVerifyLimit(ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("NativeVerifyLimit", new AndroidCallBack<LimitStatus>(handler));
        }

        public void RealnameInfoCommit(string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("RealnameInfoCommit", realname, cardID, new AndroidCallBack<LimitStatus>(handler));
        }

        public void RecallAccountDelete(ServiceCompletedHandler<VoidObject> handler)
        {
            Service.Call("RecallAccountDelete", new AndroidCallBack<VoidObject>(handler));
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<LoginResult> handler)
        {
            // TODO: 调整 - 将原为 AccessToken 类型调整为 LoginResult
            Service.Call("RegisterAccount", account, password, chkCode, new AndroidCallBack<LoginResult>(handler));
        }

        public void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler)
        {
            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
            Service.Call("RetrievePassword", account, password, chkCode, ((int)category), new AndroidCallBack<VoidObject>(handler));
        }

        public void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler)
        {
            Service.Call("GetWebPCInfo", new AndroidCallBack<WebPCInfo>(handler));
        }

        public void CommitPrivateInfo(string birthday, string sex, ServiceCompletedHandler<VoidObject> handler)
        {
            Service.Call("CommitPrivateInfo", birthday, sex, new AndroidCallBack<VoidObject>(handler));
        }

        public void GetPrivateProfile(ServiceCompletedHandler<UserPrivateInfo> handler)
        {
            Service.Call("GetPrivateProfile", new AndroidCallBack<UserPrivateInfo>(handler));
        }

    }
}

#endif