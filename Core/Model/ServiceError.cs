using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK
{
    /// <summary>
    /// 错误信息对象
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ServiceError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        public ServiceError(int code, string message)
        {
            Code = code;
            Message = message;
        }

        internal static ServiceError ModelDeserializationError => new ServiceError(-3000, "数据解析失败");
    }
}

