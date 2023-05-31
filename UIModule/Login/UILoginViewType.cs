using System;

namespace SoFunny.FunnySDK.UIModule
{
    public enum UILoginViewType
    {
        /// <summary>
        /// 登录选择页
        /// </summary>
        LoginSelect,
        /// <summary>
        /// 邮箱（手机号）登录页
        /// </summary>
        EmailOrPhonePwd,
        /// <summary>
        /// 注册与忘记密码页
        /// </summary>
        RegisterAndRetrieve,
        /// <summary>
        /// 邀请码页
        /// </summary>
        ActivationKey,
        /// <summary>
        /// 登录限制页
        /// </summary>
        LoginLimit,
        /// <summary>
        /// 实名认证（防沉迷页）
        /// </summary>
        AntiAddiction,
        /// <summary>
        /// 账号冷静期页
        /// </summary>
        CoolDownTips,
    }
}

