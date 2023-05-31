using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    internal class OtherBridge : NativeCallMethodInterface {

        private static readonly object _lock = new object();
        private static OtherBridge _instance;

        private OtherBridge() {
        }

        internal static OtherBridge GetInstance() {

            lock (_lock) {
                if (_instance == null) {
                    _instance = new OtherBridge();

                }
            }

            return _instance;
        }

        public void Call(string method) {
            Debug.LogWarning("[FunnySDK] 暂未支持该平台相关功能");
        }

        public void Call(string method, NativeParameter parameter) {
            Debug.LogWarning("[FunnySDK] 暂未支持该平台相关功能");
        }

        public void Call<T>(string method, NativeParameter parameter) {
            Debug.LogWarning("[FunnySDK] 暂未支持该平台相关功能");
        }

        public T CallReturn<T>(string method) {
            Debug.LogWarning("[FunnySDK] 暂未支持该平台相关功能");
            return default;
        }

        public T CallReturn<T>(string method, NativeParameter parameter) {
            Debug.LogWarning("[FunnySDK] 暂未支持该平台相关功能");
            return default;
        }

        public void RegisterListener() {

        }
    }
}

