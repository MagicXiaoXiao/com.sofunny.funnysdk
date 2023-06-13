using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    // 限制状态类
    [JsonObject(MemberSerialization.OptIn)]
    internal class LimitStatus
    {
        [JsonProperty("ret_code")]
        public int StatusCode;

        public StatusType Status
        {
            get
            {
                if (Enum.IsDefined(typeof(StatusType), StatusCode))
                {
                    return (StatusType)StatusCode;
                }
                else
                {
                    // 处理无效的状态码
                    return StatusType.Failed;
                }
            }
        }

        internal enum StatusType
        {
            /// <summary>
            /// 校验失败
            /// </summary>
            Failed = 0,
            /// <summary>
            /// 校验成功
            /// </summary>
            Success = 1,
            /// <summary>
            /// 校验被限制
            /// </summary>
            AllowFailed = 1000,
            /// <summary>
            /// 校验邀请码失败
            /// </summary>
            ActivationFailed = 2000,
            /// <summary>
            /// 校验邀请码不存在
            /// </summary>
            ActivationUnfilled = 2001,
            /// <summary>
            /// 校验账号正在冷静期
            /// </summary>
            AccountInCooldownFailed = 2002,
            /// <summary>
            /// 校验实名认证失败
            /// </summary>
            RealnameVerifyFailed = 2003,
            /// <summary>
            /// 校验账号已被封禁
            /// </summary>
            AccountBannedFailed = 2004,
        }
    }
}

