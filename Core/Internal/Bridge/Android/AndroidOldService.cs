#if UNITY_ANDROID
using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidOldService
    {
        private readonly AndroidJavaObject Service;

        internal AndroidOldService()
        {
            Service = new AndroidJavaObject("com.xmfunny.funnysdk.unitywrapper.FunnySdkWrapper");// Android 旧桥接对象

            RegisterListener(); // Android 旧监听对象
        }

        public void Call(string method)
        {
            Service.Call("callMethod", method);
        }

        public void Call<T>(string method, NativeParameter parameter, ServiceCompletedHandler<T> handler)
        {
            if (string.IsNullOrEmpty(parameter.GetTaskID()))
            {
                Service.Call("callMethod", method, parameter.ToJSON());
            }
            else
            {
                Service.Call("callMethod", method, parameter.ToJSON(), new AndroidOldCallback<T>(handler));
            }
        }

        public void Call(string method, NativeParameter parameter)
        {
            Service.Call("callMethod", method, parameter.ToJSON());
        }

        public T CallReturn<T>(string method)
        {
            var returnJson = Service.Call<string>("callReturn", method);
            var nativeResult = NativeResult.Create(returnJson);
            return nativeResult.TryGetValue<T>();
        }

        public T CallReturn<T>(string method, NativeParameter parameter)
        {
            var returnJson = Service.Call<string>("callReturn", method, parameter.ToJSON());
            var nativeResult = NativeResult.Create(returnJson);
            return nativeResult.TryGetValue<T>();
        }

        private void RegisterListener()
        {
            Service.Call("registerListener", new AndroidListener());
        }

    }
}
#endif

