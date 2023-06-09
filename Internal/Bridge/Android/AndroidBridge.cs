using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidBridge : IBridgeServiceBase
    {
        private AndroidJavaObject Service;

        internal AndroidBridge()
        {
            Service = new AndroidJavaObject("安卓类名路径 (后续确定)");
        }

        public void Call(string method, string parameter, IServiceAsyncCallbackHandler handler = null)
        {
            var callback = handler == null ? null : new AndroidCallBack(handler);
            Service.Call("callMethod", method, parameter, callback);
        }

    }
}

