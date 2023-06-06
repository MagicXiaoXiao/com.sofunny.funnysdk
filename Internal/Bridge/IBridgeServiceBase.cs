using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 各平台服务调用接口
    /// </summary>
    public interface IBridgeServiceBase
    {
        /// <summary>
        /// 正常调用方法（无任何返回值）
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="parameter">参数表</param>
        void Call(string method, string parameter);

        /// <summary>
        /// 异步调用方法 (包含返回值)
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="parameter">参数表</param>
        /// <param name="handler">异步处理接口</param>
        void CallAsync(string method, string parameter, IServiceAsyncCallbackHandler handler);
    }
}

