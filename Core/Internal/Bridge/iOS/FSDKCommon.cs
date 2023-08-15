#if UNITY_IOS
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKCommon : IBridgeServiceBase
    {
        public void Initialize()
        {
            FSDKCall.Builder("Initialize").Invoke();
        }

        public void ContactUS()
        {
            FSDKCall.Builder("ContactUS").Invoke();
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

        public void OpenPrivacyProtocol()
        {
            FSDKCall.Builder("OpenPrivacyProtocol").Invoke();
        }

        public void OpenUserAgreenment()
        {
            FSDKCall.Builder("OpenUserAgreenment").Invoke();
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


        public void SetLanguage(string language)
        {
            FSDKCall.Builder("SetLanguage")
                    .Add("language", language)
                    .Invoke();
        }

        public void ShowDatePicker(string date, ServiceCompletedHandler<string> handler)
        {
            FSDKCall.Builder("ShowDatePicker")
                    .Add("date", date)
                    .Invoke();
        }
    }
}

#endif
