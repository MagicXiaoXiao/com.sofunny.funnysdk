using System;

namespace SoFunny.FunnySDK.UIModule
{
    public enum UILoginPageState
    {
        /// <summary>
        /// 登录选择页
        /// </summary>
        LoginSelectPage = 100,
        /// <summary>
        /// 邮箱登录页
        /// </summary>
        EmailLoginPage,
        /// <summary>
        /// 手机登录页
        /// </summary>
        PhoneLoginPage,
        /// <summary>
        /// 账号注册页
        /// </summary>
        RegisterPage,
        /// <summary>
        /// 忘记密码页
        /// </summary>
        RetrievePage,
        /// <summary>
        /// 登录限制页
        /// </summary>
        LoginLimitPage,
        /// <summary>
        /// 账号冷静期页
        /// </summary>
        CoolDownTipsPage,
        /// <summary>
        /// 邀请码填写页
        /// </summary>
        ActivationKeyPage,
        /// <summary>
        /// 实名认证页
        /// </summary>
        AntiAddictionPage,
    }
}

