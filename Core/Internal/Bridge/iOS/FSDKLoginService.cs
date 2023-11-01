#if UNITY_IOS

using System;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKLoginService : IBridgeServiceLogin
    {
        internal FSDKLoginService()
        {

        }

        public bool IsAuthorized => FSDKCall.Builder("IsAuthorized").Invoke<bool>();

        public void ActivationCodeCommit(string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("ActivationCodeCommit")
                           .Add("code", code)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<LimitStatus> ActivationCodeCommit(string code)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("ActivationCodeCommit")
                           .Add("code", code)
                           .Then(resolve)
                           .Catch(reject)
                           .Invoke();
            });
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

        public Promise CommitPrivateInfo(string birthday, string sex)
        {
            return new Promise((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("CommitPrivateInfo")
                               .Add("birthday", birthday)
                               .Add("sex", sex)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
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

        public Promise<UserPrivateInfo> GetPrivateProfile()
        {
            return new Promise<UserPrivateInfo>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("GetPrivateProfile")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public UserProfile GetUserProfile()
        {
            return FSDKCall.Builder("GetUserProfile").Invoke<UserProfile>();
        }

        public void FetchUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            FSDKCallAndBack.Builder("FetchUserProfile")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<UserProfile> FetchUserProfile()
        {
            return new Promise<UserProfile>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("FetchUserProfile")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
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

        public Promise<WebPCInfo> GetWebPCInfo()
        {
            return new Promise<WebPCInfo>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("GetWebPCInfo")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<LoginResult> handler)
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

        public Promise<LoginResult> LoginWithCode(string account, string code)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("LoginWithCode")
                               .Add("account", account)
                               .Add("code", code)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<LoginResult> handler)
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

        public Promise<LoginResult> LoginWithPassword(string account, string password)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("LoginWithPassword")
                           .Add("account", account)
                           .Add("password", password)
                           .Then(resolve)
                           .Catch(reject)
                           .Invoke();
            });
        }

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<LoginResult> handler)
        {
            string value = provider.ToString().ToLower();

            FSDKCallAndBack.Builder("LoginWithProvider")
                           .Add("provider", value)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<LoginResult> LoginWithProvider(LoginProvider provider)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                string value = provider.ToString().ToLower();

                FSDKCallAndBack.Builder("LoginWithProvider")
                               .Add("provider", value)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void Logout()
        {
            FSDKCall.Builder("Logout").Invoke();
        }

        public void NativeVerifyLimit(ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("NativeVerifyLimit")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<LimitStatus> NativeVerifyLimit()
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("NativeVerifyLimit")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void RealnameInfoCommit(string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            FSDKCallAndBack.Builder("RealnameInfoCommit")
                           .Add("realname", realname)
                           .Add("cardID", cardID)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<LimitStatus> RealnameInfoCommit(string realname, string cardID)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("RealnameInfoCommit")
                               .Add("realname", realname)
                               .Add("cardID", cardID)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void RecallAccountDelete(ServiceCompletedHandler<VoidObject> handler)
        {
            FSDKCallAndBack.Builder("RecallAccountDelete")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise RecallAccountDelete()
        {
            return new Promise((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("RecallAccountDelete")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<LoginResult> handler)
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

        public Promise<LoginResult> RegisterAccount(string account, string password, string chkCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("RegisterAccount")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .Then(resolve)
                           .Catch(reject)
                           .Invoke();
            });
        }

        public void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler)
        {
            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

            FSDKCallAndBack.Builder("RetrievePassword")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .Add("category", (int)category)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise RetrievePassword(string account, string password, string chkCode)
        {
            return new Promise((resolve, reject) =>
            {
                CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

                FSDKCallAndBack.Builder("RetrievePassword")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .Add("category", (int)category)
                           .Then(resolve)
                           .Catch(reject)
                           .Invoke();
            });
        }

    }
}

#endif