using System;

namespace SoFunny.FunnySDK.UIModule
{
    public enum UILoginPageState
    {
        UnknownPage = -1,
        /// <summary>
        /// 登录选择页
        /// </summary>
        LoginSelectPage = 101,
        /// <summary>
        /// 账号密码登录页
        /// </summary>
        PwdLoginPage = 201,
        /// <summary>
        /// 账号验证码登录页
        /// </summary>
        CodeLoginPage = 202,
        /// <summary>
        /// 账号注册页
        /// </summary>
        RegisterPage = 203,
        /// <summary>
        /// 忘记密码页
        /// </summary>
        RetrievePage = 301,
        /// <summary>
        /// 登录限制页
        /// </summary>
        LoginLimitPage = 401,
        /// <summary>
        /// 邀请码填写页
        /// </summary>
        ActivationKeyPage = 402,
        /// <summary>
        /// 实名认证页
        /// </summary>
        AntiAddictionPage = 403,
        /// <summary>
        /// 账号冷静期页
        /// </summary>
        CoolDownTipsPage = 406,
    }
}

