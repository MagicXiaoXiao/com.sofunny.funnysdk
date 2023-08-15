#if UNITY_IOS

using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKLoginService : IBridgeServiceLogin
    {
        public void ActivationCodeCommit(string tokenValue, string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("ActivationCodeCommit")
                           .Add("tokenValue", tokenValue)
                           .Add("code", code)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void CommitPrivateInfo(string birthday, string sex, ServiceCompletedHandler<VoidObject> handler)
        {
            FSDKCallAndBack.Builder("CommitPrivateInfo")
                           .Add("birthday", birthday)
                           .Add("sex", sex)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public AccessToken GetCurrentAccessToken()
        {
            return FSDKCall.Builder("GetCurrentAccessToken").Invoke<AccessToken>();
        }

        public void GetPrivateProfile(ServiceCompletedHandler<UserPrivateInfo> handler)
        {
            FSDKCallAndBack.Builder("GetPrivateProfile")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void GetUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            FSDKCallAndBack.Builder("GetUserProfile")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler)
        {
            FSDKCallAndBack.Builder("GetWebPCInfo")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler)
        {
            FSDKCallAndBack.Builder("LoginWithCode")
                           .Add("account", account)
                           .Add("code", code)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<AccessToken> handler)
        {
            FSDKCallAndBack.Builder("LoginWithPassword")
                           .Add("account", account)
                           .Add("password", password)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<AccessToken> handler)
        {
            FSDKCallAndBack.Builder("LoginWithProvider")
                           .Add("provider", (int)provider)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void Logout()
        {
            FSDKCall.Builder("Logout").Invoke();
        }

        public void NativeVerifyLimit(string tokenValue, ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("NativeVerifyLimit")
                           .Add("tokenValue", tokenValue)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void RealnameInfoCommit(string tokenValue, string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("RealnameInfoCommit")
                           .Add("tokenValue", tokenValue)
                           .Add("realname", realname)
                           .Add("cardID", cardID)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void RecallAccountDelete(string tokenValue, ServiceCompletedHandler<VoidObject> handler)
        {
            FSDKCallAndBack.Builder("RecallAccountDelete")
                           .Add("tokenValue", tokenValue)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<AccessToken> handler)
        {
            FSDKCallAndBack.Builder("RegisterAccount")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler)
        {
            FSDKCallAndBack.Builder("RetrievePassword")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }
    }
}

#endif