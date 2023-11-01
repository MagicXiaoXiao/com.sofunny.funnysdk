﻿using System;

namespace SoFunny.FunnySDK.Internal
{
    internal struct Environment
    {
        internal const string Release = "release";
        internal const string Develop = "develop";
        internal const string Preview = "preview";
    }

    internal static class BridgeConfig
    {
        internal static string AppID { get; private set; }
        internal static bool IsMainland { get; private set; }

        internal static string BaseURL { get; private set; }
        internal static string Host { get; private set; }

        internal static string UserProtocolURL { get; private set; }
        internal static string PrivacyProtocolURL { get; private set; }

        internal static void Init(string appID, bool isMainland, string env = Environment.Develop)
        {
            AppID = appID;
            IsMainland = isMainland;
            string language = isMainland ? "zh" : "en";
            string protocolHost = IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";

            switch (env)
            {
                case Environment.Release:
                    BaseURL = isMainland ? "https://apisix-gateway.zh-cn.xmfunny.com/v1" : "https://apisix-gateway.sg.xmfunny.com/v1";
                    UserProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=USER";
                    PrivacyProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
                    break;
                case Environment.Preview:
                    BaseURL = "https://apisix-api.sofunny.io/v1";
                    Host = isMainland ? "apisix-gateway.zh-cn.xmfunny.com" : "apisix-gateway.sg.xmfunny.com";
                    UserProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=USER";
                    PrivacyProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
                    break;
                case Environment.Develop:
                    BaseURL = "http://10.30.30.93:9080/v1";
                    UserProtocolURL = $"http://10.30.30.158:8002/protocol/index.html?language={language}&client_id={appID}&category=USER";
                    PrivacyProtocolURL = $"http://10.30.30.158:8002/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
                    break;
                default: break;
            }
        }

    }
}

