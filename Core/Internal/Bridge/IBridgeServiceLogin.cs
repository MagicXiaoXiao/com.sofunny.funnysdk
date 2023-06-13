using System;

namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceLogin
    {
        /// <summary>
        /// 获取当前登录 Token
        /// </summary>
        /// <returns></returns>
        AccessToken GetCurrentAccessToken();

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="handler"></param>
        void LoginWithPassword(string account, string password, ServiceCompletedHandler<AccessToken> handler);

        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler);

        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="handler"></param>
        void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<AccessToken> handler);

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="chkCode"></param>
        /// <param name="handler"></param>
        void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<AccessToken> handler);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="chkCode"></param>
        /// <param name="handler"></param>
        void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler);

        /// <summary>
        /// Token 验证
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="handler"></param>
        void NativeVerifyLimit(string tokenValue, ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="handler"></param>
        void GetUserProfile(ServiceCompletedHandler<UserProfile> handler);
    }
}

