using System;
using SoFunny.FunnySDK.Internal;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    internal static class BridgeConfig
    {
        internal static string AppID { get; private set; }
        internal static bool IsMainland { get; private set; }

        internal static string BaseURL { get; private set; }
        internal static string Host { get; private set; }

        internal static string UserProtocolURL { get; private set; }
        internal static string PrivacyProtocolURL { get; private set; }

        //internal static void Init(string appID, bool isMainland, string env = Environment.Release)
        //{
        //    AppID = appID;
        //    IsMainland = isMainland;
        //    string language = isMainland ? "zh" : "en";
        //    string protocolHost = IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";

        //    switch (env)
        //    {
        //        case Environment.Release:
        //            BaseURL = isMainland ? "https://apisix-gateway.zh-cn.xmfunny.com/v1" : "https://apisix-gateway.sg.xmfunny.com/v1";
        //            UserProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=USER";
        //            PrivacyProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
        //            break;
        //        case Environment.Preview:
        //            BaseURL = "https://apisix-api.sofunny.io/v1";
        //            Host = isMainland ? "apisix-gateway.zh-cn.xmfunny.com" : "apisix-gateway.sg.xmfunny.com";
        //            UserProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=USER";
        //            PrivacyProtocolURL = $"https://{protocolHost}/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
        //            break;
        //        case Environment.Develop:
        //            BaseURL = "http://10.30.30.93:9080/v1";
        //            UserProtocolURL = $"http://10.30.30.158:8002/protocol/index.html?language={language}&client_id={appID}&category=USER";
        //            PrivacyProtocolURL = $"http://10.30.30.158:8002/protocol/index.html?language={language}&client_id={appID}&category=SECRET";
        //            break;
        //        default: break;
        //    }
        //}

        internal static void Init(NativeConfig config)
        {
            if (config is null)
            {
                Logger.LogError("无法读取 SDK 配置数据，请检查接入步骤是否正确");
                return;
            }

            Logger.Log($"SDK 当前环境 - {config.Env}");

            AppID = config.AppID;
            IsMainland = config.Env == PackageEnv.Mainland;
            string language = "zh";

            switch (config.Env)
            {
                case PackageEnv.Mainland: // 国内配置
                    BaseURL = "https://apisix-gateway.zh-cn.xmfunny.com/v1";
                    UserProtocolURL = $"https://auth.zh-cn.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=USER";
                    PrivacyProtocolURL = $"https://auth.zh-cn.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=SECRET";
                    break;
                case PackageEnv.Overseas: // 海外配置
                    language = "en";
                    BaseURL = "https://apisix-gateway.sg.xmfunny.com/v1";
                    UserProtocolURL = $"https://auth.sg.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=USER";
                    PrivacyProtocolURL = $"https://auth.sg.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=SECRET";
                    break;
                case PackageEnv.InternalBeta: // 内服测试配置
                case PackageEnv.InvitationBeta: // 邀请测试配置
                    BaseURL = "https://apisix-gateway.zh-cn.xmfunny.com/v1";
                    UserProtocolURL = $"https://auth.zh-cn.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=USER";
                    PrivacyProtocolURL = $"https://auth.zh-cn.xmfunny.com/protocol/index.html?language={language}&client_id={AppID}&category=SECRET";
                    break;
                default: break;
            }

        }
    }
}

