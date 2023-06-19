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
        /// 登出账号
        /// </summary>
        void Logout();

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
        /// 提交邀请码
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        void ActivationCodeCommit(string tokenValue, string code, ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 提交实名信息
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="realname"></param>
        /// <param name="cardID"></param>
        /// <param name="handler"></param>
        void RealnameInfoCommit(string tokenValue, string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 撤销账号删除
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="handler"></param>
        void RecallAccountDelete(string tokenValue, ServiceCompletedHandler<VoidObject> handler);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="handler"></param>
        void GetUserProfile(ServiceCompletedHandler<UserProfile> handler);

        /// <summary>
        /// 获取个人中心用户信息
        /// </summary>
        /// <param name="handler"></param>
        void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler);
    }
}

