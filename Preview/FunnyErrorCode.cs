using System;

namespace SoFunny.FunnySDKPreview
{

    internal enum FunnyErrorCode
    {

        /// <summary>
        /// 登录验证已无效或已过期
        /// </summary>
        LOGIN_INVALID_OR_EXPIRED = 401,

        /// <summary>
        /// 没有登录账号
        /// </summary>
        NOT_LOGIN = 1002,

        /// <summary>
        /// 没有对应授权渠道
        /// </summary>
        //NO_AUTH_CHANNEL = 3001,

        /// <summary>
        /// 登录过程被主动取消
        /// </summary>
        LOGIN_CANCEL = 3003,

        /// <summary>
        /// Token 已失效
        /// </summary>
        ACCESSTOKEN_INVALID = 34000,

        /// <summary>
        /// 隐私信息授权被取消
        /// </summary>
        PRIVACY_PROFILE_CANCEL = -1000,

        /// <summary>
        /// 隐私信息授权未开启
        /// </summary>
        PRIVACY_PROFILE_NOT_ENABLE = -1001,

        /// <summary>
        /// 隐私信息授权出错
        /// </summary>
        PRIVACY_PROFILE_ERROR = -1002,
    }

}

