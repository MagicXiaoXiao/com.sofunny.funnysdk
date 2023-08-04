using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;


namespace SoFunny.FunnySDKPreview
{

#if UNITY_IOS

    internal delegate void iOSCallback(string taskID, string jsonResult);
    internal delegate void iOSListener(string identifier, string jsonValue);

    internal class iOSBridge : NativeCallMethodInterface {
        private static readonly object _lock = new object();
        private static iOSBridge _instance;
        private Dictionary<string, INativeResultDecodeHandler> handlerDic;
        private iOSBridge() {
            handlerDic = new Dictionary<string, INativeResultDecodeHandler>();
        }

        internal static iOSBridge GetInstance() {

            lock (_lock) {
                if (_instance == null) {
                    _instance = new iOSBridge();

                }
            }

            return _instance;
        }

        public void Call(string method) {
            funny_sdk_call(method, string.Empty);
        }

        public void Call(string method, NativeParameter parameter) {
            funny_sdk_call(method, parameter.ToJSON());
        }

        public void Call<T>(string method, NativeParameter parameter) {
            if (string.IsNullOrEmpty(parameter.GetTaskID())) {
                funny_sdk_call(method, parameter.ToJSON());
            }
            else {
                handlerDic.Add(parameter.GetTaskID(), new iOSResultHandler<T>());
                var callObject = new NativeiOSCallModel(method, parameter);
                funny_sdk_closure(callObject.ToJSON(), OnCallbackHandler);
            }

        }

        public T CallReturn<T>(string method) {
            var jsonResult = funny_sdk_callreturn(method, string.Empty);
            var nativeResult = NativeResult.Create(jsonResult);
            return nativeResult.TryGetValue<T>();
        }

        public T CallReturn<T>(string method, NativeParameter parameter) {
            var jsonResult = funny_sdk_callreturn(method, parameter.ToJSON());
            var nativeResult = NativeResult.Create(jsonResult);
            return nativeResult.TryGetValue<T>();
        }


        public void RegisterListener() {
            funny_sdk_listener(OnListenerHandler);
        }

        [MonoPInvokeCallback(typeof(iOSListener))]
        internal static void OnListenerHandler(string identifier, string jsonValue) {
            FunnySDK.OnNativeListener(identifier, jsonValue);
        }

        [MonoPInvokeCallback(typeof(iOSCallback))]
        internal static void OnCallbackHandler(string taskID, string jsonResult) {
            if (!string.IsNullOrEmpty(jsonResult)) {
                var result = NativeResult.Create(jsonResult);

                if (_instance.handlerDic.ContainsKey(taskID)) {

                    var handler = _instance.handlerDic[taskID];

                    handler.DecodeResult(taskID, result);

                    _instance.handlerDic.Remove(taskID);
                }
            }
        }

        [DllImport("__Internal", EntryPoint = "FunnyUnityCallMethod")]
        private static extern void funny_sdk_call(string method, string jsonParameter);
        [DllImport("__Internal", EntryPoint = "FunnyUnityClosureMethod")]
        private static extern void funny_sdk_closure(string jsonObject, iOSCallback callback);
        [DllImport("__Internal", EntryPoint = "FunnyUnityRegisterListener")]
        private static extern void funny_sdk_listener(iOSListener listener);
        [DllImport("__Internal", EntryPoint = "FunnyUnityCallReturn")]
        private static extern string funny_sdk_callreturn(string method, string jsonParameter);


    }



    internal class NativeiOSCallModel {

        private Dictionary<string, object> dic;

        internal NativeiOSCallModel(string method, NativeParameter parameter) {
            dic = new Dictionary<string, object>();
            dic["method"] = method;
            dic["param"] = parameter.getMainDic();
        }

        internal string ToJSON() {
            return Json.Serialize(dic);
        }
    }
#endif
}

