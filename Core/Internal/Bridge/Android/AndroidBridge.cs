#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class AndroidBridge : IBridgeServiceBase, IBridgeServiceTrack
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
        internal AndroidOldService OldService;

        private AndroidBridge()
        {
            Service = new AndroidJavaObject("com.xmfunny.funnysdk.unitywrapper.internal.unity.FunnySdkWrapper4Unity");
            OldService = new AndroidOldService();
        }

        public void Initialize()
        {
            Service.Call("Initialize", BridgeConfig.AppID);
            OldService.Call("setupSDK");
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
            Service.Call("SendVerificationCode", account, ((int)codeAction), ((int)codeCategory), new AndroidCallBack<VoidObject>(handler));
        }

        public void TrackEvent(Track track)
        {
            // Debug.Log("reportEvent TrackEvent: " + track.Name + " json: " + track.JsonData());
            Service.Call("TrackData", track.JsonData(), track.Name);
        }

        public void ShowDatePicker(string date, ServiceCompletedHandler<string> handler)
        {
            // Tips: 取消则传值空字符串
            Service.Call("ShowDatePicker", date, new AndroidCallBack<string>(handler));
        }

        public void SetLanguage(string language)
        {
            // language 参数，英文 = en , 简体中文 = zh
            Service.Call("SetLanguage", language);
        }

        public void OpenAgreenment()
        {
            OldService.Call("openProtocol");
        }
    }
}

#endif
