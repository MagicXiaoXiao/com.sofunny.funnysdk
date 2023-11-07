#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using SoFunny.FunnySDK.Promises;
using Newtonsoft.Json;

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

        public NativeConfig GetNativeConfig()
        {
            try
            {
                string jsonString = Service.CallStatic<string>("GetNativeConfig");
                return JsonConvert.DeserializeObject<NativeConfig>(jsonString);
            }
            catch (Exception ex)
            {
                Logger.LogError($"数据解析异常: {ex.Message}");
                return null;
            }
        }

        public void ContactUS()
        {
            Service.Call("OpenFeedback"); // 打开问题反馈页
        }

        public void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler)
        {
            Service.Call("GetAppInfo", new AndroidCallBack<AppInfoConfig>(handler));
        }

        public Promise<AppInfoConfig> GetAppInfo()
        {
            return new Promise<AppInfoConfig>((resolve, reject) =>
            {
                Service.Call("GetAppInfo", new AndroidCallBack<AppInfoConfig>(resolve, reject));
            });
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

        public Promise SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory)
        {
            return new Promise((resolve, reject) =>
            {
                Service.Call("SendVerificationCode", account, ((int)codeAction), ((int)codeCategory), new AndroidCallBack(resolve, reject));
            });
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

        public Promise<string> ShowDatePicker(string date)
        {
            return new Promise<string>((resolve, reject) =>
            {
                Service.Call("ShowDatePicker", date, new AndroidCallBack<string>(resolve, reject));
            });
        }

        public void SetLanguage(string language)
        {
            Service.Call("SetLanguage", language);
        }

        public void OpenAgreenment()
        {
            OldService.Call("openProtocol");
        }

    }
}

#endif
