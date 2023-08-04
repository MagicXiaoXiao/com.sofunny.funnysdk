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

        [JsonProperty("code")]
        private readonly int errorCode;

        [JsonProperty("message")]
        private readonly string errorMessage;

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code => errorCode;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message => errorMessage;

        /// <summary>
        /// 错误类型
        /// </summary>
        public ServiceErrorType Error
        {
            get
            {
                if (Enum.TryParse<ServiceErrorType>(Code.ToString(), out var enumValue))
                {
                    return enumValue;
                }
                else
                {
                    return ServiceErrorType.UnknownError;
                }
            }
        }

        public ServiceError(int code, string message)
        {
            errorCode = code;
            errorMessage = message;
        }

        internal static ServiceError Make(ServiceErrorType errorType)
        {
            string message = "发生未知错误";
            switch (errorType)
            {
                case ServiceErrorType.InvalidAccessToken:
                    message = "当前登录信息已失效，请重新登录";
                    break;
                case ServiceErrorType.ServerOccurredFailed:
                    message = "服务器响应失败，请稍后再试";
                    break;
                case ServiceErrorType.ConnectToServerFailed:
                    message = "服务器连接出错";
                    break;
                case ServiceErrorType.ProcessingDataFailed:
                    message = "数据解析失败";
                    break;
                case ServiceErrorType.NoLoginError:
                    message = "未登录，请先进行登录操作";
                    break;
                default: break;
            }

            return new ServiceError((int)errorType, message);
        }

    }

    /// <summary>
    /// SDK 服务错误类型
    /// </summary>
    public enum ServiceErrorType
    {
        /// <summary>
        /// Token 令牌失效（需重新登录授权）
        /// </summary>
        InvalidAccessToken = 401,
        /// <summary>
        /// SDK 服务器发生错误
        /// </summary>
        ServerOccurredFailed = 500,
        /// <summary>
        /// 无法连接服务器（弱网、无网情况下可能发生）
        /// </summary>
        ConnectToServerFailed = -1000,
        /// <summary>
        /// 处于未登录状态
        /// </summary>
        NoLoginError = -2000,
        /// <summary>
        /// 数据处理失败 (数据解析失败、或 SDK 内部处理发生错误)
        /// </summary>
        ProcessingDataFailed = -9000,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = -1,
    }

}

