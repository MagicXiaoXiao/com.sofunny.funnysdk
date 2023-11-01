#if UNITY_IOS
using System;
using AOT;
using System.Runtime.InteropServices;
using SoFunny.FunnySDK.Promises;
using UnityEditor;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKCommon : IBridgeServiceBase
    {
        internal FSDKCommon()
        {

        }

        private delegate void NotificationMessage(string name, string jsonString);

        [DllImport("__Internal")]
        private static extern void FSDK_NotificationCenter(NotificationMessage message);

        [MonoPInvokeCallback(typeof(NotificationMessage))]
        protected static void PostNotificationHandler(string name, string jsonString)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogWarning("native event name is empty!");
                return;
            }
            BridgeNotificationCenter.Default.Post(name, BridgeValue.Create(jsonString));
        }

        public void Initialize()
        {
            // 初始化
            FSDKCall.Builder("Initialize").Invoke();
            // 注册通知中心
            FSDK_NotificationCenter(PostNotificationHandler);
        }

        public void ContactUS()
        {
            FSDKCall.Builder("OpenFeedback").Invoke();
        }

        public void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler)
        {
            FSDKCallAndBack.Builder("GetAppInfo")
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<AppInfoConfig> GetAppInfo()
        {
            return new Promise<AppInfoConfig>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("GetAppInfo")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void OpenPrivacyProtocol()
        {
            FSDKCall.Builder("OpenWebView")
                    .Add("url", BridgeConfig.PrivacyProtocolURL)
                    .Invoke();
        }

        public void OpenUserAgreenment()
        {
            FSDKCall.Builder("OpenWebView")
                    .Add("url", BridgeConfig.UserProtocolURL)
                    .Invoke();
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            FSDKCallAndBack.Builder("SendVerificationCode")
                           .Add("account", account)
                           .Add("codeAction", (int)codeAction)
                           .Add("codeCategory", (int)codeCategory)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory)
        {
            return new Promise((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("SendVerificationCode")
                               .Add("account", account)
                               .Add("codeAction", (int)codeAction)
                               .Add("codeCategory", (int)codeCategory)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void SetLanguage(string language)
        {
            FSDKCall.Builder("SetLanguage")
                    .Add("language", language)
                    .Invoke();
        }

        public void ShowDatePicker(string date, ServiceCompletedHandler<string> handler)
        {
            FSDKCallAndBack.Builder("ShowDatePicker")
                           .Add("date", date)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public Promise<string> ShowDatePicker(string date)
        {
            return new Promise<string>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("ShowDatePicker")
                               .Add("date", date)
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
        }

        public void OpenAgreenment()
        {
            FSDKCall.Builder("OpenAgreenment").Invoke();
        }
    }
}

#endif
