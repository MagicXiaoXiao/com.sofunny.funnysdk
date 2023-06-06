using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal class Method
    {
        private string Name;
        private Dictionary<string, object> Parameters;
        private IBridgeServiceBase BridgeService;

        internal Method(string name) : this(name, new Dictionary<string, object>())
        {
        }

        internal Method(string name, Dictionary<string, object> keyValues)
        {
            Name = name;
            Parameters = keyValues;

#if UNITY_ANDROID && !UNITY_EDITOR
            BridgeService = new AndroidBridge();
#else
            BridgeService = new UnknownBridge();
#endif

        }

        /// <summary>
        /// 添加方法参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal void AddParameter(string key, object value)
        {
            Parameters.Add(key, value);
        }

        internal void Call()
        {
            var param = JsonConvert.SerializeObject(Parameters);
            BridgeService.Call(Name, param);
        }

        internal void CallAsync(IServiceAsyncCallbackHandler callbackHandler)
        {
            var param = JsonConvert.SerializeObject(Parameters);
            BridgeService.CallAsync(Name, param, callbackHandler);
        }
    }
}

