using System;

namespace SoFunny.FunnySDK
{
    /// <summary>
    /// 绑定类型
    /// </summary>
    public enum BindingType
    {
        /// <summary>
        /// 未知渠道
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 脸书
        /// </summary>
        Facebook = 1,

        /// <summary>
        /// 推特
        /// </summary>
        Twitter = 2,

        /// <summary>
        /// 谷歌账号
        /// </summary>
        Google = 3,

        /// <summary>
        /// 苹果账号
        /// </summary>
        AppleID = 4,

        /// <summary>
        /// 邮箱
        /// </summary>
        Email = 6,

        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 7,

        /// <summary>
        /// 腾讯QQ
        /// </summary>
        QQ = 8,

        /// <summary>
        /// 手机号
        /// </summary>
        Phone = 9,

        /// <summary>
        /// TapTap
        /// </summary>
        TapTap = 10,
    }
}

