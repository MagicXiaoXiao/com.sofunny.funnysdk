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

        public void ContactUS()
        {
            throw new NotImplementedException();
        }

        public void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OpenPriacyProtocol()
        {
            throw new NotImplementedException();
        }

        public void OpenUserAgreenment()
        {
            throw new NotImplementedException();
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            throw new NotImplementedException();
        }

        //public void Call<T>(string method, Dictionary<string, object> parameter, ServiceCompletedHandler<T> handler = null)
        //{
        //    var callback = handler == null ? null : new AndroidCallBack<T>(handler);
        //    var param = JsonConvert.SerializeObject(parameter);

        //    Service.Call("callMethod", method, param, callback);
        //}
    }
}

