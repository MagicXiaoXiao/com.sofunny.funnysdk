﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class AndroidBridge : IBridgeServiceBase
    {
        private static readonly object _lock = new object();
        private static AndroidBridge _instance;

        internal static AndroidBridge GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new AndroidBridge();
                }
            }

            return _instance;
        }

        private AndroidJavaObject Service;

        private AndroidBridge()
        {
            Service = new AndroidJavaObject("安卓类名路径 (后续确定)");
        }

        public void Initialize()
        {
            Service.Call("Initialize", BridgeConfig.AppID);
        }

        public void ContactUS()
        {
            Service.Call("OpenFeedback"); // 打开问题反馈页
        }

        public void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler)
        {
            Service.Call("GetAppInfo", new AndroidCallBack<AppInfoConfig>(handler));
        }

        public void OpenPrivacyProtocol()
        {
            Service.Call("OpenPrivacyProtocol");
        }

        public void OpenUserAgreenment()
        {
            Service.Call("OpenUserAgreenment");
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            Service.Call("SendVerificationCode", account, codeAction, codeCategory, new AndroidCallBack<VoidObject>(handler));
        }

    }
}

