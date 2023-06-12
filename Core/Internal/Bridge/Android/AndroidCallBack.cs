using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// Android 回调接口处理对象
    /// </summary>
    internal class AndroidCallBack<T> : AndroidJavaProxy
    {
        private ServiceCompletedHandler<T> callbackHandler;

        internal AndroidCallBack(ServiceCompletedHandler<T> handler) : base("安卓 Java 接口对象路径 (后续确定)")
        {
            callbackHandler = handler;
        }

        // 等待后续接口方法定义

    }
}

