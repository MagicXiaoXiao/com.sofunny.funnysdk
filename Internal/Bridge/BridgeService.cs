using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    public class BridgeService
    {
        private IBridgeServiceBase Service;

        public BridgeService(string appID, bool isMainland)
        {
            BridgeConfig.Init(appID, isMainland);

#if UNITY_ANDROID && !UNITY_EDITOR
            Service = new AndroidBridge();
#else
            Service = new PCBridge();
#endif
        }

        public void Call(string method, Dictionary<string, object> parameters = null, IServiceAsyncCallbackHandler handler = null)
        {
            var param = JsonConvert.SerializeObject(parameters);
            Service.Call(method, param, handler);
        }
    }
}

