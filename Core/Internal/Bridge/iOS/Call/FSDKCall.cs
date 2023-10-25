#if UNITY_IOS

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;


namespace SoFunny.FunnySDK.Internal
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class FSDKCall
    {
        /*
         * 模型解析如下所示:
         * 
         * {
	     *   "method": "rawValue",
	     *   "parameters": {
		 *       "key1": "obj1",
		 *       "key2": "obj2"
	     *   }
         * }
         *
         */

        [JsonProperty("method")]
        private readonly string rawValue;

        [JsonProperty("parameters")]
        private Dictionary<string, object> parameters;

        private FSDKCall(string rawValue)
        {
            this.rawValue = rawValue;
            this.parameters = new Dictionary<string, object>();
        }

        internal static FSDKCall Builder(string rawValue)
        {
            return new FSDKCall(rawValue);
        }

        internal FSDKCall Add(string key, object value)
        {
            parameters.Add(key, value);
            return this;
        }

        internal void Invoke()
        {
            string json = JsonConvert.SerializeObject(this);

            Logger.Log($"发起服务 {rawValue} 调用 - {json}");

            FSDK_Call(json);
        }

        internal T Invoke<T>()
        {
            string json = JsonConvert.SerializeObject(this);

            Logger.Log($"发起服务 {rawValue} 调用 - {json}");

            string reValue = FSDK_CallAndReturn(json);

            Logger.Log($"服务 {rawValue} 返回结果 - {reValue}");

            return JsonConvert.DeserializeObject<T>(reValue);
        }


        [DllImport("__Internal")]
        private static extern void FSDK_Call(string json);

        [DllImport("__Internal")]
        private static extern string FSDK_CallAndReturn(string json);
    }
}

#endif