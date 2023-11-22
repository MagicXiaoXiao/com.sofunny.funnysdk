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

        public Promise<LoginResult> LoginWithCode(string account, string code)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("LoginWithCode")
                               .Add("account", account)
                               .Add("code", code)
                               .Then<LoginResult>((result) =>
                               {
                                   FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                                   resolve(result);
                               })
                               .Catch(reject)
                               .Invoke();
            });
        }

        public Promise<LoginResult> LoginWithPassword(string account, string password)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("LoginWithPassword")
                           .Add("account", account)
                           .Add("password", password)
                           .Then<LoginResult>((result) =>
                           {
                               FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                               resolve(result);
                           })
                           .Catch(reject)
                           .Invoke();
            });
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

        public Promise<LoginResult> RegisterAccount(string account, string password, string chkCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("RegisterAccount")
                           .Add("account", account)
                           .Add("password", password)
                           .Add("chkCode", chkCode)
                           .Then<LoginResult>((result) =>
                           {
                               FunnyDataStore.AddAccountRcord(new LoginAccountRecord(account));
                               resolve(result);
                           })
                           .Catch(reject)
                           .Invoke();
            });
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