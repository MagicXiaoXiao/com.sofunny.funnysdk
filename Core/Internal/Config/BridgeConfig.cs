using System;

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

        internal static void Init(string appID, bool isMainland, string env = Environment.Develop)
        {
            AppID = appID;
            IsMainland = isMainland;

            switch (env)
            {
                case Environment.Release:
                    BaseURL = isMainland ? "https://apisix-gateway.zh-cn.xmfunny.com/v1" : "https://apisix-gateway.sg.xmfunny.com/v1";
                    break;
                case Environment.Preview:
                    BaseURL = "https://apisix-api.sofunny.io/v1";
                    Host = isMainland ? "apisix-gateway.zh-cn.xmfunny.com" : "apisix-gateway.sg.xmfunny.com";
                    break;
                case Environment.Develop:
                    BaseURL = "http://10.30.30.93:9080/v1";
                    break;
                default: break;
            }
        }

    }
}

