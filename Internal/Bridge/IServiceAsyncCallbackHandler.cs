using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 异步服务调用处理接口
    /// </summary>
    public interface IServiceAsyncCallbackHandler
    {
        /// <summary>
        /// 成功处理方法
        /// </summary>
        /// <param name="modelJSON">JSON 字符串</param>
        void OnSuccessHandler(string modelJSON);

        /// <summary>
        /// 失败处理方法
        /// </summary>
        /// <param name="error">ServiceError</param>
        void OnErrorHandler(ServiceError error);
    }
}

