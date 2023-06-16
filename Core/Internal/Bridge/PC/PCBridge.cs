using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class PCBridge : IBridgeServiceBase
    {

        private static readonly object _lock = new object();
        private static PCBridge _instance;

        private PCBridge() { }

        internal static PCBridge GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new PCBridge();
                }
            }

            return _instance;
        }

        public void ContactUS()
        {
            Logger.Log("PC 平台暂无此功能");
        }

        public void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler)
        {
            Network.Send(new AppInfoRequest(), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        AppInfoConfig config = JsonConvert.DeserializeObject<AppInfoConfig>(data);
                        handler?.Invoke(config, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析出错 - " + ex.Message);
                        handler?.Invoke(null, ServiceError.ModelDeserializationError);
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void Initialize()
        {
            Logger.Log("PC 端初始化完毕");
        }

        public void OpenPrivacyProtocol()
        {
            string host = BridgeConfig.IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";
            Application.OpenURL($"https://{host}/privacy-policy?hide_back_button=true");
        }

        public void OpenUserAgreenment()
        {
            string host = BridgeConfig.IsMainland ? "account.zh-cn.xmfunny.com" : "account.sg.xmfunny.com";
            Application.OpenURL($"https://{host}/service-protocol?hide_back_button=true");
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            if (BridgeConfig.IsMainland)
            {
                // 国内获取票据
                Network.Send(new TicketRequest(), (data, error) =>
                {
                    if (error == null)
                    {
                        var json = JObject.Parse(data);
                        string ticket = json["ticket"].ToString();
                        // 发送验证码
                        CreateCode(account, codeAction, codeCategory, ticket, handler);
                    }
                    else
                    {
                        handler?.Invoke(null, error);
                    }
                });
            }
            else
            {
                // 海外直接发送验证码
                CreateCode(account, codeAction, codeCategory, "", handler);
            }
        }

        private void CreateCode(string account, CodeAction codeAction, CodeCategory codeCategory, string ticket, ServiceCompletedHandler<VoidObject> handler)
        {
            Network.Send(new SendCodeRequest(account, codeAction, codeCategory, ticket), (data, error) =>
            {
                if (error == null)
                {
                    handler?.Invoke(new VoidObject(), null);
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }
    }
}

