using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidBridge : IBridgeServiceBase
    {
        private AndroidJavaObject Service;

        internal AndroidBridge()
        {
            Service = new AndroidJavaObject("安卓类名路径 (后续确定)");
        }

        public void Initialize()
        {

        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {

        }

        //public void Call<T>(string method, Dictionary<string, object> parameter, ServiceCompletedHandler<T> handler = null)
        //{
        //    var callback = handler == null ? null : new AndroidCallBack<T>(handler);
        //    var param = JsonConvert.SerializeObject(parameter);

        //    Service.Call("callMethod", method, param, callback);
        //}
    }
}

