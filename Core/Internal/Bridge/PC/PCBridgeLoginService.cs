#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class PCBridge : IBridgeServiceLogin
    {
        public AccessToken GetCurrentAccessToken()
        {
            return FunnyDataStore.GetCurrentToken();
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<AccessToken> handler)
        {
            Network.Send(new LoginWithCodeRequest(account, code), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        // 解析 Model 数据
                        SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                        RedirectHandler(ssoToken, handler);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
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
                    try
                    {
                        // 解析 Model 数据
                        SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                        RedirectHandler(ssoToken, handler);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }

                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        private void RedirectHandler(SSOToken token, ServiceCompletedHandler<AccessToken> handler)
        {
            var redirectRequest = new RedirectRequest(token.Value);

            Network.Send(redirectRequest, (redirect, error) =>
            {
                if (error == null)
                {
                    JObject json = JObject.Parse(redirect);
                    if (json.TryGetValue("location", out var location))
                    {
                        string locationValue = location.ToString();
                        string code = locationValue.GetQuery("code");
                        NativeTokenHandler(code, redirectRequest.pkce, handler);
                    }
                    else
                    {
                        Logger.LogError("数据解析失败");
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        private void NativeTokenHandler(string code, PKCE pkce, ServiceCompletedHandler<AccessToken> handler)
        {
            Network.Send(new NativeTokenRequest(code, pkce), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(data);
                        handler?.Invoke(accessToken, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void NativeVerifyLimit(string tokenValue, ServiceCompletedHandler<LimitStatus> handler)
        {
            Network.Send(new NativeVerifyLimitRequest(tokenValue), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);
                        handler?.Invoke(limitStatus, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<AccessToken> handler)
        {
            handler?.Invoke(null, new ServiceError(-1, "PC 版本暂无此登录"));
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<AccessToken> handler)
        {
            Network.Send(new RegisterAccountRequest(account, password, chkCode), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                        RedirectHandler(ssoToken, handler);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
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

        public void GetUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            AccessToken token = GetCurrentAccessToken();
            if (token == null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new UserProfileRequest(token.Value), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(data);
                        handler?.Invoke(userProfile, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.Log("数据解析出错 - " + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void ActivationCodeCommit(string tokenValue, string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            Network.Send(new ActivationCodeRequest(tokenValue, code), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);
                        handler?.Invoke(limitStatus, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void RealnameInfoCommit(string tokenValue, string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            Network.Send(new RealnameCommitRequest(tokenValue, realname, cardID), (data, error) =>
            {
                if (error == null)
                {
                    try
                    {
                        LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);
                        handler?.Invoke(limitStatus, null);
                    }
                    catch (JsonException ex)
                    {
                        Logger.LogError("数据解析失败。" + ex.Message);
                        handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    }
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void RecallAccountDelete(string tokenValue, ServiceCompletedHandler<VoidObject> handler)
        {
            Network.Send(new RecallAccountDeleteRequest(tokenValue), (data, error) =>
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

        public void Logout()
        {
            FunnyDataStore.DeleteToken();
        }

        /// <summary>
        /// 获取 PC Info 内容
        /// </summary>
        /// <param name="handler"></param>
        public void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler)
        {
            AccessToken accessToken = GetCurrentAccessToken();

            if (accessToken is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new PCTokenRequest(accessToken.Value), (data, error) =>
            {
                if (error == null)
                {
                    JObject bodyJson = JObject.Parse(data);
                    string pcToken = bodyJson["access_token"].Value<string>();
                    FetchPCInfo(pcToken, handler);
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        private void FetchPCInfo(string pcToken, ServiceCompletedHandler<WebPCInfo> handler)
        {

            Network.Send(new PCInfoRequest(pcToken), (data, error) =>
            {
                if (error == null)
                {
                    WebPCInfo info = JsonConvert.DeserializeObject<WebPCInfo>(data);
                    handler?.Invoke(info, null);
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void CommitPrivateInfo(string birthday, string sex, ServiceCompletedHandler<VoidObject> handler)
        {
            AccessToken accessToken = GetCurrentAccessToken();

            if (accessToken is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new ProfileTokenRequest(accessToken.Value), (data, error) =>
            {
                if (error == null)
                {
                    JObject bodyJson = JObject.Parse(data);
                    string pcToken = bodyJson["access_token"].Value<string>();

                    Network.Send(new PutPrivateInfoRequest(pcToken, sex, birthday), (_, commitError) =>
                    {
                        if (commitError == null)
                        {
                            handler?.Invoke(new VoidObject(), null);
                        }
                        else
                        {
                            handler?.Invoke(null, commitError);
                        }
                    });
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void GetPrivateProfile(ServiceCompletedHandler<UserPrivateInfo> handler)
        {
            AccessToken accessToken = GetCurrentAccessToken();

            if (accessToken is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new ProfileTokenRequest(accessToken.Value), (data, error) =>
            {
                if (error == null)
                {
                    JObject bodyJson = JObject.Parse(data);
                    string pcToken = bodyJson["access_token"].Value<string>();

                    Network.Send(new GetPrivateInfoRequest(pcToken), (profileData, profileError) =>
                    {
                        if (profileError == null)
                        {
                            var info = JsonConvert.DeserializeObject<UserPrivateInfo>(profileData);
                            handler?.Invoke(info, null);
                        }
                        else
                        {
                            handler?.Invoke(null, profileError);
                        }
                    });
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }
    }
}

#endif