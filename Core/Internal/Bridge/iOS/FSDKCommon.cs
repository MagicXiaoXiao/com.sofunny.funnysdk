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

        public void OpenPrivacyProtocol()
        {
            string host = BridgeConfig.IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";

            FSDKCall.Builder("OpenWebView")
                    .Add("url", $"https://{host}/privacy-policy?hide_back_button=true")
                    .Invoke();
        }

        public void OpenUserAgreenment()
        {
            string host = BridgeConfig.IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";

            FSDKCall.Builder("OpenWebView")
                    .Add("url", $"https://{host}/service-protocol?hide_back_button=true")
                    .Invoke(); ;
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
            FSDKCallAndBack.Builder("ShowDatePicker")
                           .Add("date", date)
                           .AddCallbackHandler((success, json) =>
                           {
                               IosHelper.HandlerServiceCallback(success, json, handler);
                           })
                           .Invoke();
        }

        public void OpenAgreenment()
        {
            FSDKCall.Builder("OpenAgreenment").Invoke();
        }
    }
}

#endif
