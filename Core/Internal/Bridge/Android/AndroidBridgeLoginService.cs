using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class AndroidBridge : IBridgeServiceLogin
    {
        public void ActivationCodeCommit(string tokenValue, string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("ActivationCodeCommit", tokenValue, code, new AndroidCallBack<LimitStatus>(handler));
        }

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

        public void GetUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            Service.Call("GetUserProfile", new AndroidCallBack<UserProfile>(handler));
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler)
        {
            Service.Call("LoginWithCode", account, code, new AndroidCallBack<AccessToken>(handler));
        }

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<AccessToken> handler)
        {
            Service.Call("LoginWithPassword", account, password, new AndroidCallBack<AccessToken>(handler));
        }

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<AccessToken> handler)
        {
            Service.Call("LoginWithProvider", ((int)provider), new AndroidCallBack<AccessToken>(handler));
        }

        public void Logout()
        {
            Service.Call("Logout");
        }

        public void NativeVerifyLimit(string tokenValue, ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("NativeVerifyLimit", tokenValue, new AndroidCallBack<LimitStatus>(handler));
        }

        public void RealnameInfoCommit(string tokenValue, string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            Service.Call("RealnameInfoCommit", tokenValue, realname, cardID, new AndroidCallBack<LimitStatus>(handler));
        }

        public void RecallAccountDelete(string tokenValue, ServiceCompletedHandler<VoidObject> handler)
        {
            Service.Call("RecallAccountDelete", tokenValue, new AndroidCallBack<VoidObject>(handler));
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<AccessToken> handler)
        {
            Service.Call("RegisterAccount", account, password, chkCode, new AndroidCallBack<AccessToken>(handler));
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

    }
}

