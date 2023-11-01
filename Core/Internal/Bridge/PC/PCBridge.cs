#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using SoFunny.FunnySDK.Promises;

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
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public Promise<AppInfoConfig> GetAppInfo()
        {
            return new Promise<AppInfoConfig>((resolve, reject) =>
            {
                Network.Send(new AppInfoRequest(), (data, error) =>
                {
                    if (error == null)
                    {
                        try
                        {
                            AppInfoConfig config = JsonConvert.DeserializeObject<AppInfoConfig>(data);
                            resolve(config);
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析出错 - " + ex.Message);
                            reject(ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                        }
                    }
                    else
                    {
                        reject(error);
                    }
                });
            });
        }

        public void Initialize()
        {
            Logger.Log("PC 端初始化完毕");
        }

        public void OpenPrivacyProtocol()
        {
            Application.OpenURL(BridgeConfig.PrivacyProtocolURL);
        }

        public void OpenUserAgreenment()
        {
            Application.OpenURL(BridgeConfig.UserProtocolURL);
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
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

        public Promise SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory)
        {

            return new Promise((resolve, reject) =>
            {
                Network.Send(new TicketRequest(), (data, error) =>
                {
                    if (error == null)
                    {
                        var json = JObject.Parse(data);
                        string ticket = json["ticket"].ToString();
                        // 发送验证码
                        CreateCode(account, codeAction, codeCategory, ticket)
                        .Then(resolve)
                        .Catch(reject);
                    }
                    else
                    {
                        reject(error);
                    }
                });
            });

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

        public Promise CreateCode(string account, CodeAction codeAction, CodeCategory codeCategory, string ticket)
        {
            return new Promise((resolve, reject) =>
            {
                Network.Send(new SendCodeRequest(account, codeAction, codeCategory, ticket), (data, error) =>
                {
                    if (error == null)
                    {
                        resolve();
                    }
                    else
                    {
                        reject(error);
                    }
                });
            });
        }

        public void TrackEvent(Track track)
        {
            // PC 埋点暂不实现
        }

        public void ShowDatePicker(string date, ServiceCompletedHandler<string> handler)
        {
            // PC 暂不实现
            Logger.Log("PC 暂未开发相关功能");
            handler?.Invoke("2000-01-01", null);
        }

        public Promise<string> ShowDatePicker(string date)
        {
            return new Promise<string>((resolve, reject) =>
            {
                resolve("2000-01-01");
            });
        }

        public void SetLanguage(string language)
        {
            Logger.Log("PC 语言已设置为 - " + language);
        }

        public void OpenAgreenment()
        {
            Logger.LogWarning("PC 或 Editor 暂未开发此功能");
        }
    }
}

#endif