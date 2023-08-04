using System;

namespace SoFunny.FunnySDK
{
    /// <summary>
    /// 第三方登录提供商
    /// </summary>
    public enum LoginProvider
    {
        Unknown = -1,

        Email = 101,
        Phone = 103,
        Guest = 102,

        Google = 204,
        Apple = 201,
        Facebook = 205,
        Twitter = 206,

        WeChat = 202,
        QQ = 203,
        TapTap = 207
    }
}

