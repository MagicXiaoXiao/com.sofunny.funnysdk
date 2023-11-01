using System;
using SoFunny.FunnySDK.Promises;

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
        Promise<LoginResult> LoginWithPassword(string account, string password);

        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        Promise<LoginResult> LoginWithCode(string account, string code);

        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <param name="provider"></param>
        Promise<LoginResult> LoginWithProvider(LoginProvider provider);

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="chkCode"></param>
        Promise<LoginResult> RegisterAccount(string account, string password, string chkCode);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="chkCode"></param>
        Promise RetrievePassword(string account, string password, string chkCode);

        /// <summary>
        /// Token 验证
        /// </summary>
        Promise<LimitStatus> NativeVerifyLimit();

        /// <summary>
        /// 提交邀请码
        /// </summary>
        /// <param name="code"></param>
        Promise<LimitStatus> ActivationCodeCommit(string code);

        /// <summary>
        /// 提交实名信息
        /// </summary>
        /// <param name="realname"></param>
        /// <param name="cardID"></param>
        Promise<LimitStatus> RealnameInfoCommit(string realname, string cardID);

        /// <summary>
        /// 撤销账号删除
        /// </summary>
        Promise RecallAccountDelete();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        UserProfile GetUserProfile();

        /// <summary>
        /// 从远端拉取用户信息
        /// </summary>
        Promise<UserProfile> FetchUserProfile();

        /// <summary>
        /// 获取隐私信息
        /// </summary>
        Promise<UserPrivateInfo> GetPrivateProfile();

        /// <summary>
        /// 提交隐私信息
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="sex"></param>
        Promise CommitPrivateInfo(string birthday, string sex);

        /// <summary>
        /// 获取个人中心用户信息
        /// </summary>
        Promise<WebPCInfo> GetWebPCInfo();
    }
}

