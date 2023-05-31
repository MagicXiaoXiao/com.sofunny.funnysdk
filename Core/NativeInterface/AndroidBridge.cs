using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK
{
#if UNITY_ANDROID
    internal class AndroidListener : AndroidJavaProxy {
        internal AndroidListener() : base("com.xmfunny.funnysdk.unitywrapper.listener.FunnySdkListener") {

        }

        internal void Post(string identifier) {
            FunnySDK.OnNativeListener(identifier);
        }

        internal void Post(string identifier, string jsonValue) {
            FunnySDK.OnNativeListener(identifier, jsonValue);
        }

    }

    internal class AndroidCallback<T> : AndroidJavaProxy {
        internal AndroidCallback() : base("com.xmfunny.funnysdk.unitywrapper.listener.FunnySdkCallback") {

        }

        internal void OnComplete(string taskID, string jsonResult) {
            if (!string.IsNullOrEmpty(jsonResult)) {
                var result = JsonUtility.FromJson<NativeResult>(jsonResult);

                if (result.success) {
                    var value = result.TryGetValue<T>();
                    InteropTasks.TrySetResultAndRemove(taskID, value);

                }
                else {
                    var exception = JsonUtility.FromJson<FunnyError>(result.jsonValue);
                    InteropTasks.TrySetExceptionAndRemove<T>(taskID, exception.MatchException());
                }
            }
        }

    }

    internal class AndroidBridge : NativeCallMethodInterface {
        private static readonly object _lock = new object();
        private static AndroidBridge _instance;

        private AndroidJavaObject sdkWrapper;

        private AndroidBridge() {
            if (sdkWrapper == null) {
                sdkWrapper = new AndroidJavaObject("com.xmfunny.funnysdk.unitywrapper.FunnySdkWrapper");
            }
        }

        internal static AndroidBridge GetInstance() {

            lock (_lock) {
                if (_instance == null) {
                    _instance = new AndroidBridge();
                }
            }

            return _instance;
        }

        public void Call(string method) {
            sdkWrapper.Call("callMethod", method);
        }

        public void Call<T>(string method, NativeParameter parameter) {
            if (string.IsNullOrEmpty(parameter.GetTaskID())) {
                sdkWrapper.Call("callMethod", method, parameter.ToJSON());
            }
            else {
                sdkWrapper.Call("callMethod", method, parameter.ToJSON(), new AndroidCallback<T>());
            }

        }

        public void Call(string method, NativeParameter parameter) {
            sdkWrapper.Call("callMethod", method, parameter.ToJSON());
        }

        public T CallReturn<T>(string method) {
            var returnJson = sdkWrapper.Call<string>("callReturn", method);
            var nativeResult = NativeResult.Create(returnJson);
            return nativeResult.TryGetValue<T>();
        }

        public T CallReturn<T>(string method, NativeParameter parameter) {
            var returnJson = sdkWrapper.Call<string>("callReturn", method, parameter.ToJSON());
            var nativeResult = NativeResult.Create(returnJson);
            return nativeResult.TryGetValue<T>();
        }

        public void RegisterListener() {
            sdkWrapper.Call("registerListener", new AndroidListener());
        }
    }
#endif
}

