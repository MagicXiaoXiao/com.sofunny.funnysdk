using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal delegate void iOSServiceHandler(string serviceId, bool success, string json);

    [JsonObject(MemberSerialization.OptIn)]
    internal class FSDKCallAndBack
    {
        /*
         * 模型解析如下所示:
         * 
         * {
	     *   "method": "rawValue",
	     *   "serviceId": "UUID",
	     *   "parameters": {
		 *       "key1": "obj1",
		 *       "key2": "obj2"
	     *   }
         * }
         *
         */

        private static Dictionary<string, Action<bool, string>> _callback = new Dictionary<string, Action<bool, string>>();

        [JsonProperty("method")]
        private readonly string rawValue;

        [JsonProperty("parameters")]
        private Dictionary<string, object> parameters;

        [JsonProperty("serviceId")]
        private string serviceId;

        private FSDKCallAndBack(string rawValue)
        {
            this.rawValue = rawValue;
            this.parameters = new Dictionary<string, object>();
            this.serviceId = "";
        }

        internal static FSDKCallAndBack Builder(string rawValue)
        {
            return new FSDKCallAndBack(rawValue);
        }

        internal FSDKCallAndBack Add(string key, object value)
        {
            parameters.Add(key, value);

            return this;
        }

        internal FSDKCallAndBack AddCallbackHandler(Action<bool, string> handler)
        {
            serviceId = Guid.NewGuid().ToString("N");
            _callback.Add(serviceId, handler);

            return this;
        }

        internal void Invoke()
        {
            string json = JsonConvert.SerializeObject(this);

            Logger.Log($"发起服务 {rawValue} - {serviceId} 调用 - {json}");

            FSDK_CallAndBack(json, CallbackResult);
        }

        [DllImport("__Internal")]
        private static extern void FSDK_CallAndBack(string json, iOSServiceHandler handler);

        [MonoPInvokeCallback(typeof(iOSServiceHandler))]
        protected static void CallbackResult(string serviceId, bool success, string json)
        {
            if (string.IsNullOrEmpty(serviceId))
            {
                Logger.LogError($"服务 - {serviceId} 未添加执行处理结果的对象");
                return;
            }

            if (_callback.TryGetValue(serviceId, out var handler))
            {
                Logger.Log($"收到服务{serviceId} 的回调结果 - {success} - {json}");
                handler.Invoke(success, json);

                _callback.Remove(serviceId);
            }
            else
            {
                Logger.LogError("未找到 serviceId 相关的回调处理对象");
            }
        }
    }
}

