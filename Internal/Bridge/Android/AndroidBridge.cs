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

        /* 
         * method 为调用的方法名
         * 
         * 
         * parameter 为附加参数表（JSON 字符串形式）
         * parameter 案例如下:
         * {
         *  "key1":"value1",
         *  "key2": 0,
         *  "key3": true
         * }
         * Android 桥接包可将其转化为 Map 等相关对象来接收
         */

        public void Call(string method, string parameter)
        {
            // 实现 Android 方法调用桥接逻辑 (无返回值形式，携带 2 参数)
            Service.Call(method, parameter);
        }

        public void CallAsync(string method, string parameter, IServiceAsyncCallbackHandler handler)
        {
            // 实现 Android 方法调用桥接逻辑 (返回结果形式，携带 3 参数)
            Service.Call(method, parameter, new AndroidCallBack(handler));
        }
    }
}

