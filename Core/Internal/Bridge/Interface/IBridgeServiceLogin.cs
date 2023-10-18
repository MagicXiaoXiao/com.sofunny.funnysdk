using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 登录功能接口
    /// </summary>
    internal interface IBridgeServiceLogin
    {
        /// <summary>
        /// 是否进行过登录授权
        /// </summary>
        bool IsAuthorized { get; }

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
        void LoginWithPassword(string account, string password, ServiceCompletedHandler<LoginResult> handler);

        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        void LoginWithCode(string account, string code, ServiceCompletedHandler<LoginResult> handler);

        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="handler"></param>
        void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<LoginResult> handler);

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="chkCode"></param>
        /// <param name="handler"></param>
        void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<LoginResult> handler);

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
        void NativeVerifyLimit(ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 提交邀请码
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        void ActivationCodeCommit(string code, ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 提交实名信息
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="realname"></param>
        /// <param name="cardID"></param>
        /// <param name="handler"></param>
        void RealnameInfoCommit(string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler);

        /// <summary>
        /// 撤销账号删除
        /// </summary>
        /// <param name="tokenValue"></param>
        /// <param name="handler"></param>
        void RecallAccountDelete(ServiceCompletedHandler<VoidObject> handler);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        UserProfile GetUserProfile();

        /// <summary>
        /// 从远端拉取用户信息
        /// </summary>
        /// <param name="handler"></param>
        void FetchUserProfile(ServiceCompletedHandler<UserProfile> handler);

        /// <summary>
        /// 获取隐私信息
        /// </summary>
        /// <param name="handler"></param>
        void GetPrivateProfile(ServiceCompletedHandler<UserPrivateInfo> handler);

        /// <summary>
        /// 提交隐私信息
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="sex"></param>
        /// <param name="handler"></param>
        void CommitPrivateInfo(string birthday, string sex, ServiceCompletedHandler<VoidObject> handler);

        /// <summary>
        /// 获取个人中心用户信息
        /// </summary>
        /// <param name="handler"></param>
        void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler);

    }
}

