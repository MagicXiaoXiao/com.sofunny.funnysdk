using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 错误信息对象
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; private set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; private set; }

        internal ServiceError(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}

