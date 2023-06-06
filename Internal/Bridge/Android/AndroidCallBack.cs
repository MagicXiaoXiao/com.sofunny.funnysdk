using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// Android 回调接口处理对象
    /// </summary>
    internal class AndroidCallBack : AndroidJavaProxy
    {
        private IServiceAsyncCallbackHandler callbackHandler;

        internal AndroidCallBack(IServiceAsyncCallbackHandler handler) : base("安卓 Java 接口对象路径 (后续确定)")
        {
            callbackHandler = handler;
        }

        // 成功处理
        internal void OnSuccessHandler(string jsonModel)
        {
            callbackHandler.OnSuccessHandler(jsonModel);
        }

        // 失败处理
        internal void OnErrorHandler(int code, string message)
        {
            callbackHandler.OnErrorHandler(new ServiceError(code, message));
        }
    }
}

