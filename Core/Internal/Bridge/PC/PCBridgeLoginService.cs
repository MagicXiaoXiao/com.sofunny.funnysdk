﻿#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal partial class PCBridge : IBridgeServiceLogin
    {
        private AccessToken _current;
        private UserProfile _userProfile;

        public bool IsAuthorized => FunnyDataStore.HasToken;

        public AccessToken GetCurrentAccessToken()
        {
            return _current;
        }

        public void LoginWithCode(string account, string code, ServiceCompletedHandler<LoginResult> handler)
        {
            Network.Send(new LoginWithCodeRequest(account, code), (data, error) =>
            {
                if (error is null)
                {
                    try
                    {
                        // 解析 Model 数据
                        LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                        if (!loginResult.IsNeedBind)
                        {
                            SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                            FunnyDataStore.UpdateToken(ssoToken);
                        }

                        handler?.Invoke(loginResult, null);
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

        public Promise<LoginResult> LoginWithCode(string account, string code)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Network.Send(new LoginWithCodeRequest(account, code), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            // 解析 Model 数据
                            LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                            if (!loginResult.IsNeedBind)
                            {
                                SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                                FunnyDataStore.UpdateToken(ssoToken);
                            }

                            resolve(loginResult);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        public void LoginWithPassword(string account, string password, ServiceCompletedHandler<LoginResult> handler)
        {
            Network.Send(new LoginWithPasswordRequest(account, password), (data, error) =>
            {
                if (error is null)
                {
                    try
                    {
                        // 解析 Model 数据
                        LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                        if (!loginResult.IsNeedBind)
                        {
                            SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                            FunnyDataStore.UpdateToken(ssoToken);
                        }

                        handler?.Invoke(loginResult, null);
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

        public Promise<LoginResult> LoginWithPassword(string account, string password)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Network.Send(new LoginWithPasswordRequest(account, password), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            // 解析 Model 数据
                            LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                            if (!loginResult.IsNeedBind)
                            {
                                SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                                FunnyDataStore.UpdateToken(ssoToken);
                            }

                            resolve(loginResult);
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        //public Promise<LoginResult> LoginWithPassword(string account, string password)
        //{
        //    return new Promise<LoginResult>((resolve, reject) =>
        //    {
        //        Network.Send(new LoginWithPasswordRequest(account, password), (data, error) =>
        //        {
        //            if (error is null)
        //            {
        //                try
        //                {
        //                    // 解析 Model 数据
        //                    LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
        //                    if (!loginResult.IsNeedBind)
        //                    {
        //                        SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
        //                        FunnyDataStore.UpdateToken(ssoToken);
        //                    }

        //                    resolve(loginResult);
        //                }
        //                catch (JsonException ex)
        //                {
        //                    Logger.LogError("数据解析失败。" + ex.Message);
        //                    reject(ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
        //                }
        //            }
        //            else
        //            {
        //                reject(error);
        //            }
        //        });

        //    });
        //}

        //private void RedirectHandler(SSOToken token, ServiceCompletedHandler<AccessToken> handler)
        //{
        //    var redirectRequest = new RedirectRequest(token.Value);

        //    Network.Send(redirectRequest, (redirect, error) =>
        //    {
        //        if (error is null)
        //        {
        //            JObject json = JObject.Parse(redirect);
        //            if (json.TryGetValue("location", out var location))
        //            {
        //                string locationValue = location.ToString();
        //                string code = locationValue.GetQuery("code");
        //                NativeTokenHandler(code, redirectRequest.pkce, handler);
        //            }
        //            else
        //            {
        //                Logger.LogError("数据解析失败");
        //                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
        //            }
        //        }
        //        else
        //        {
        //            handler?.Invoke(null, error);
        //        }
        //    });
        //}

        //private void NativeTokenHandler(string code, PKCE pkce, ServiceCompletedHandler<AccessToken> handler)
        //{
        //    Network.Send(new NativeTokenRequest(code, pkce), (data, error) =>
        //    {
        //        if (error is null)
        //        {
        //            try
        //            {
        //                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(data);
        //                handler?.Invoke(accessToken, null);
        //            }
        //            catch (JsonException ex)
        //            {
        //                Logger.LogError("数据解析失败。" + ex.Message);
        //                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
        //            }
        //        }
        //        else
        //        {
        //            handler?.Invoke(null, error);
        //        }
        //    });
        //}

        public void NativeVerifyLimit(ServiceCompletedHandler<LimitStatus> handler)
        {
            string token = FunnyDataStore.GetCurrentToken().Value;

            Network.Send(new NativeVerifyLimitRequest(token), (data, error) =>
            {
                if (error is null)
                {
                    try
                    {
                        LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);

                        if (limitStatus.Status == LimitStatus.StatusType.Success)
                        {
                            AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(data);

                            _current = accessToken;

                            FetchUserProfile((profile, fetchError) =>
                            {
                                if (fetchError == null)
                                {
                                    handler?.Invoke(limitStatus, null);
                                }
                                else
                                {
                                    handler?.Invoke(null, fetchError);
                                }
                            });
                        }
                        else
                        {
                            handler?.Invoke(limitStatus, null);
                        }
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

        public Promise<LimitStatus> NativeVerifyLimit()
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                string token = FunnyDataStore.GetCurrentToken().Value;

                Network.Send(new NativeVerifyLimitRequest(token), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);

                            if (limitStatus.Status == LimitStatus.StatusType.Success)
                            {
                                AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(data);

                                _current = accessToken;

                                FetchUserProfile()
                                .Then((profile) =>
                                {
                                    resolve(limitStatus);
                                })
                                .Catch((fetchError) =>
                                {
                                    reject(fetchError);
                                });
                            }
                            else
                            {
                                resolve(limitStatus);
                            }
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        public void LoginWithProvider(LoginProvider provider, ServiceCompletedHandler<LoginResult> handler)
        {
            handler?.Invoke(null, new ServiceError(-1, "PC 版本暂无此登录"));
        }

        public Promise<LoginResult> LoginWithProvider(LoginProvider provider)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                reject(new ServiceError(-1, "PC 版本暂无此登录"));
            });
        }

        public void RegisterAccount(string account, string password, string chkCode, ServiceCompletedHandler<LoginResult> handler)
        {
            Network.Send(new RegisterAccountRequest(account, password, chkCode), (data, error) =>
            {
                if (error is null)
                {
                    try
                    {
                        LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                        if (!loginResult.IsNeedBind)
                        {
                            SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                            FunnyDataStore.UpdateToken(ssoToken);
                        }

                        handler?.Invoke(loginResult, null);
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

        public Promise<LoginResult> RegisterAccount(string account, string password, string chkCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                Network.Send(new RegisterAccountRequest(account, password, chkCode), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            LoginResult loginResult = JsonConvert.DeserializeObject<LoginResult>(data);
                            if (!loginResult.IsNeedBind)
                            {
                                SSOToken ssoToken = JsonConvert.DeserializeObject<SSOToken>(data);
                                FunnyDataStore.UpdateToken(ssoToken);
                            }

                            resolve(loginResult);
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        public void RetrievePassword(string account, string password, string chkCode, ServiceCompletedHandler<VoidObject> handler)
        {
            CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

            Network.Send(new RetrievePasswordRequest(account, password, chkCode, category), (data, error) =>
            {
                if (error is null)
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

        public Promise RetrievePassword(string account, string password, string chkCode)
        {
            return new Promise((resolve, reject) =>
            {
                CodeCategory category = BridgeConfig.IsMainland ? CodeCategory.Phone : CodeCategory.Email;

                Network.Send(new RetrievePasswordRequest(account, password, chkCode, category), (data, error) =>
                {
                    if (error is null)
                    {
                        Logger.Log("找回密码成功 - " + data);
                        resolve();
                    }
                    else
                    {
                        Logger.LogError("找回失败: " + error.Message);
                        reject(error);
                    }
                });
            });
        }

        public UserProfile GetUserProfile()
        {
            return _userProfile;
        }

        public void FetchUserProfile(ServiceCompletedHandler<UserProfile> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new UserProfileRequest(token.Value), (data, error) =>
            {
                if (error is null)
                {
                    try
                    {
                        UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(data);

                        _userProfile = userProfile;

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

        public Promise<UserProfile> FetchUserProfile()
        {
            return new Promise<UserProfile>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new UserProfileRequest(token.Value), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(data);

                            _userProfile = userProfile;

                            resolve(userProfile);
                        }
                        catch (JsonException ex)
                        {
                            Logger.Log("数据解析出错 - " + ex.Message);
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

        public void ActivationCodeCommit(string code, ServiceCompletedHandler<LimitStatus> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new ActivationCodeRequest(token.Value, code), (data, error) =>
            {
                if (error is null)
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

        public Promise<LimitStatus> ActivationCodeCommit(string code)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new ActivationCodeRequest(token.Value, code), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);
                            resolve(limitStatus);
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        public void RealnameInfoCommit(string realname, string cardID, ServiceCompletedHandler<LimitStatus> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new RealnameCommitRequest(token.Value, realname, cardID), (data, error) =>
            {
                if (error is null)
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

        public Promise<LimitStatus> RealnameInfoCommit(string realname, string cardID)
        {
            return new Promise<LimitStatus>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new RealnameCommitRequest(token.Value, realname, cardID), (data, error) =>
                {
                    if (error is null)
                    {
                        try
                        {
                            LimitStatus limitStatus = JsonConvert.DeserializeObject<LimitStatus>(data);
                            resolve(limitStatus);
                        }
                        catch (JsonException ex)
                        {
                            Logger.LogError("数据解析失败。" + ex.Message);
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

        public void RecallAccountDelete(ServiceCompletedHandler<VoidObject> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new RecallAccountDeleteRequest(token.Value), (data, error) =>
            {
                if (error is null)
                {
                    handler?.Invoke(new VoidObject(), null);
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public Promise RecallAccountDelete()
        {
            return new Promise((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new RecallAccountDeleteRequest(token.Value), (data, error) =>
                {
                    if (error is null)
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

        public void Logout()
        {
            _current = null;
            _userProfile = null;
            FunnyDataStore.DeleteToken();
        }

        /// <summary>
        /// 获取 PC Info 内容
        /// </summary>
        /// <param name="handler"></param>
        public void GetWebPCInfo(ServiceCompletedHandler<WebPCInfo> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            FetchPCInfo(token.Value, handler);
        }

        public Promise<WebPCInfo> GetWebPCInfo()
        {
            return new Promise<WebPCInfo>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                FetchPCInfo(token.Value).Then(resolve).Catch(reject);
            });
        }

        private void FetchPCInfo(string pcToken, ServiceCompletedHandler<WebPCInfo> handler)
        {

            Network.Send(new PCInfoRequest(pcToken), (data, error) =>
            {
                if (error is null)
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

        private Promise<WebPCInfo> FetchPCInfo(string pcToken)
        {
            return new Promise<WebPCInfo>((resolve, reject) =>
            {
                Network.Send(new PCInfoRequest(pcToken), (data, error) =>
                {
                    if (error is null)
                    {
                        WebPCInfo info = JsonConvert.DeserializeObject<WebPCInfo>(data);
                        resolve(info);
                    }
                    else
                    {
                        reject(error);
                    }
                });
            });
        }

        public void CommitPrivateInfo(string birthday, string sex, ServiceCompletedHandler<VoidObject> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new PutPrivateInfoRequest(token.Value, sex, birthday), (_, commitError) =>
            {
                if (commitError is null)
                {
                    handler?.Invoke(new VoidObject(), null);
                }
                else
                {
                    handler?.Invoke(null, commitError);
                }
            });
        }

        public Promise CommitPrivateInfo(string birthday, string sex)
        {
            return new Promise((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new PutPrivateInfoRequest(token.Value, sex, birthday), (_, commitError) =>
                {
                    if (commitError is null)
                    {
                        resolve();
                    }
                    else
                    {
                        reject(commitError);
                    }
                });
            });
        }

        public void GetPrivateProfile(ServiceCompletedHandler<UserPrivateInfo> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new GetPrivateInfoRequest(token.Value), (profileData, profileError) =>
            {
                if (profileError is null)
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

        public Promise<UserPrivateInfo> GetPrivateProfile()
        {
            return new Promise<UserPrivateInfo>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.GetCurrentToken();

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new GetPrivateInfoRequest(token.Value), (profileData, profileError) =>
                {
                    if (profileError is null)
                    {
                        var info = JsonConvert.DeserializeObject<UserPrivateInfo>(profileData);
                        resolve(info);
                    }
                    else
                    {
                        reject(profileError);
                    }
                });
            });
        }

    }
}

#endif