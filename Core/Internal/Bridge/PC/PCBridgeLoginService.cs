using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class PCBridge : IBridgeServiceLogin
    {
        public AccessToken GetCurrentAccessToken()
        {
            return default;
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler)
        {
            Network.Send(new LoginWithCodeRequest(account, code), (data, error) =>
            {
                if (error == null)
                {
                    Logger.Log("登录成功 - " + data);
                    // 解析 Model 数据
                    SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);

                    Network.Send(new RedirectRequest(ssoToken.Value), (redirect, nerror) =>
                    {
                        if (error == null)
                        {
                            Logger.Log("Redirect成功 - " + redirect);
                        }
                        else
                        {
                            Logger.Log("登录失败 - " + nerror.Message);
                        }
                    });
                }
                else
                {
                    Logger.Log("登录失败 - " + error.Message);
                    handler?.Invoke(null, error);
                }
            });
        }

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<AccessToken> handler)
        {
            Network.Send(new LoginWithPasswordRequest(account, password), (data, error) =>
            {
                if (error == null)
                {
                    Logger.Log("登录成功 - " + data);
                    // 解析 Model 数据
                    SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);

                    Network.Send(new RedirectRequest(ssoToken.Value), (redirect, nerror) =>
                    {
                        if (error == null)
                        {
                            Logger.Log("Redirect成功 - " + redirect);
                        }
                        else
                        {
                            Logger.Log("登录失败 - " + nerror.Message);
                        }
                    });
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void LoginWithProvider(string channel, ServiceCompletedHandler<AccessToken> handler)
        {
            throw new NotImplementedException();
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<SSOToken> handler)
        {
            Network.Send(new RegisterAccountRequest(account, password, chkCode), (data, error) =>
            {
                if (error == null)
                {
                    Logger.Log("注册成功 - " + data);
                    try
                    {
                        SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                        handler?.Invoke(ssoToken, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("注册失败: " + ex.Message);
                        handler?.Invoke(null, new ServiceError(-3000, "数据解析失败"));
                    }
                }
                else
                {
                    Logger.LogError("注册失败: " + error.Message);
                    handler?.Invoke(null, error);
                }
            });
        }

        public void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler)
        {
            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

            Network.Send(new RetrievePasswordRequest(account, password, chkCode, category), (data, error) =>
            {
                if (error == null)
                {
                    Logger.Log("找回密码成功 - " + data);
                    handler?.Invoke(new VoidObject(), null);
                }
                else
                {
                    Logger.LogError("找回失败: " + error.Message);
                    handler?.Invoke(null, error);
                }
            });
        }
    }
}

