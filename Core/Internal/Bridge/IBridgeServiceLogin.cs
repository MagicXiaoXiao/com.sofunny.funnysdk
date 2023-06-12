using System;

namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceLogin
    {
        AccessToken GetCurrentAccessToken();

        void LoginWithPassword(string account, string password, ServiceCompletedHandler<AccessToken> handler);
        void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler);
        void LoginWithProvider(string channel, ServiceCompletedHandler<AccessToken> handler);

        void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<SSOToken> handler);
        void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler);
    }
}

