using System;
using UnityEngine;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyLoginService : IFunnyLoginAPI
    {
        private readonly FunnySDKConfig Config;
        private readonly IBridgeServiceLogin LoginBridgeService;
        private readonly IBridgeServiceBase BaseBridgeService;
        private ILoginServiceDelegate LoginDelegate;

        internal FunnyLoginService(FunnySDKConfig config, IBridgeServiceBase baseBridgeService, IBridgeServiceLogin loginBridgeService)
        {
            Config = config;
            BaseBridgeService = baseBridgeService;
            LoginBridgeService = loginBridgeService;

        }

        public void GetUserProfile(IUserServiceDelegate serviceDelegate)
        {
            LoginBridgeService.GetUserProfile((userProfile, error) =>
            {
                if (error == null)
                {
                    serviceDelegate?.OnUserProfileSuccess(userProfile);
                }
                else
                {
                    serviceDelegate?.OnUserProfileFailure(error);
                }
            });
        }

        public void StartFlow(ILoginServiceDelegate serviceDelegate)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android 平台暂不显示指示器 UI
#else
            Loader.ShowIndicator();
#endif

            // 设置代理对象
            LoginDelegate = serviceDelegate;
            AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

            BaseBridgeService.GetAppInfo((appConfig, error) =>
            {
                if (error == null)
                {
                    UIService.Login.SetupLoginConfig(this, appConfig.GetLoginProviders());

                    if (accessToken != null)
                    {
                        // 已登录，验证 Token
                        VerifyLimit(accessToken);
                    }
                    else
                    {
                        Loader.HideIndicator();
                        UIService.Login.Open();
                    }
                }
                else
                {
                    Loader.HideIndicator();
                    LoginDelegate?.OnLoginFailure(error);
                }
            });
        }
    }

    #region 登录服务接口实现

    internal partial class FunnyLoginService : ILoginViewEvent
    {
        public void OnClickPriacyProtocol()
        {
            BaseBridgeService.OpenPrivacyProtocol();
        }

        public void OnClickUserAgreenment()
        {
            BaseBridgeService.OpenUserAgreenment();
        }

        public void OnCloseView(UILoginPageState pageState)
        {
            Logger.Log("关闭了登录页" + pageState);
            LoginDelegate?.OnLoginCancel();
            LoginDelegate = null;
        }

        public void OnLoginWithCode(string account, string code)
        {
            Logger.Log($"发起了验证码登录 - {account} - {code}");
            Loader.ShowIndicator();

            LoginBridgeService.LoginWithCode(account, code, (token, error) =>
            {
                if (error == null)
                {
                    // 数据存储
                    FunnyDataStore.UpdateToken(token);
                    // 验证 Token
                    VerifyLimit(token, true);
                }
                else
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }
            });

        }

        public void OnLoginWithPassword(string account, string password)
        {
            Logger.Log($"发起了账号密码登录 - {account} - {password}");
            Loader.ShowIndicator();
            LoginBridgeService.LoginWithPassword(account, password, (token, error) =>
            {
                if (error == null)
                {
                    // 数据存储
                    FunnyDataStore.UpdateToken(token);
                    // 验证 Token
                    VerifyLimit(token);
                }
                else
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }
            });
        }

        /// <summary>
        /// 验证 Token
        /// </summary>
        /// <param name="token"></param>
        private void VerifyLimit(AccessToken token, bool auto = false)
        {
            LoginBridgeService.NativeVerifyLimit(token.Value, (limitStatus, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    LimitResultHandler(limitStatus);
                }
                else
                {
                    Toast.ShowFail(error.Message);

                    if (auto)
                    {
                        LoginDelegate?.OnLoginFailure(error);
                    }
                }
            });
        }

        /// <summary>
        /// 限制结果处理
        /// </summary>
        /// <param name="limitStatus"></param>
        private void LimitResultHandler(LimitStatus limitStatus)
        {
            switch (limitStatus.Status)
            {
                case LimitStatus.StatusType.Success:
                    {
                        // UI 展示逻辑
                        Toast.ShowSuccess("登录成功");
                        // 关闭 UI 以及后续逻辑
                        UIService.Login.CloseView();

                        AccessToken token = LoginBridgeService.GetCurrentAccessToken();
                        LoginDelegate?.OnLoginSuccess(token);
                        LoginDelegate = null;
                    }
                    break;
                case LimitStatus.StatusType.AccountBannedFailed:
                    {
                        Loader.ShowIndicator();

                        LoginBridgeService.GetWebPCInfo((pcInfo, error) =>
                        {
                            Loader.HideIndicator();

                            WebPCInfo info;

                            if (error == null)
                            {
                                info = pcInfo;
                            }
                            else
                            {
                                info = new WebPCInfo();
                                Toast.ShowFail(error.Message);
                                Logger.LogError(error.Message);
                            }

                            string tips = $"您的账号已被封停，账号将在 {info.BanDate} 之后解除封停。如有疑问，请联系客服";

                            if (pcInfo.UnblockedAt == -1)
                            {
                                tips = "您的账号已永久封停，如有疑问，请联系客服";
                            }
                            // 账号已被封禁处理
                            UIService.Login.JumpTo(UILoginPageState.LoginLimitPage, tips);
                        });
                    }

                    break;
                case LimitStatus.StatusType.AllowFailed:
                    // IP 限制页面
                    UIService.Login.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
                case LimitStatus.StatusType.AccountInCooldownFailed:
                    {
                        Loader.ShowIndicator();

                        LoginBridgeService.GetWebPCInfo((pcInfo, error) =>
                        {
                            Loader.HideIndicator();

                            WebPCInfo info;

                            if (error == null)
                            {
                                info = pcInfo;
                            }
                            else
                            {
                                info = new WebPCInfo();
                                Logger.LogError(error.Message);
                                Toast.ShowFail(error.Message);
                            }

                            string tips = $"账号 {info.Account} 于 {info.StartDate} 提交了永久删除账号申请，将于 {info.DeadlineDate} 永久删除。如需要保留账号，请点击下方按钮撤回申请";

                            // 账号冷静期页面
                            UIService.Login.JumpTo(UILoginPageState.CoolDownTipsPage, tips);
                        });
                    }
                    break;
                case LimitStatus.StatusType.ActivationFailed:
                    Toast.ShowFail("无效邀请码");
                    UIService.Login.JumpTo(UILoginPageState.ActivationKeyPage);
                    break;
                case LimitStatus.StatusType.ActivationUnfilled:
                    // 跳转邀请码页面
                    UIService.Login.JumpTo(UILoginPageState.ActivationKeyPage);
                    break;
                default:
                    Toast.ShowFail("未知限制");
                    UIService.Login.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
            }
        }

        /// <summary>
        /// 获取冷静期显示相关信息
        /// </summary>
        private void CooldownVerifyHandler()
        {

        }

        public void OnLoginWithProvider(LoginProvider provider)
        {
            Logger.Log("发起了第三方登录 - " + provider);
            Loader.ShowIndicator();
            LoginBridgeService.LoginWithProvider(provider, (accessToken, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    VerifyLimit(accessToken);
                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });
        }

        public void OnRegisterAccount(string account, string pwd, string code)
        {
            Loader.ShowIndicator();
            LoginBridgeService.RegisterAccount(account, pwd, code, (accessToken, error) =>
            {
                if (error == null)
                {
                    // 数据存储
                    FunnyDataStore.UpdateToken(accessToken);
                    // 验证 Token
                    VerifyLimit(accessToken);
                }
                else
                {
                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }
            });

        }

        public void OnRetrievePassword(string account, string newPwd, string code)
        {
            Loader.ShowIndicator();
            LoginBridgeService.RetrievePassword(account, newPwd, code, (none, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    Toast.ShowSuccess("修改成功");
                    // 跳转页面
                    UIService.Login.JumpTo(Config.IsMainland ? UILoginPageState.PhoneLoginPage : UILoginPageState.EmailLoginPage);
                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });

        }

        public void OnSendVerifcationCode(string account, UILoginPageState pageState)
        {
            switch (pageState)
            {
                case UILoginPageState.RegisterPage: // 注册新账号
                    {
                        UIService.Login.TimerSending(pageState); // 显示发送中状态

                        CodeCategory category = Config.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
                        BaseBridgeService.SendVerificationCode(account, CodeAction.Signup, category, (data, error) =>
                        {
                            if (error == null)
                            {
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                UIService.Login.TimerReset(pageState); // 失败还原状态
                                Toast.ShowFail(error.Message);
                            }
                        });
                    }
                    break;
                case UILoginPageState.RetrievePage: // 找回密码
                    {
                        UIService.Login.TimerSending(pageState); // 显示发送中状态

                        CodeCategory category = Config.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
                        BaseBridgeService.SendVerificationCode(account, CodeAction.ChangePassword, category, (data, error) =>
                        {
                            if (error == null)
                            {
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                UIService.Login.TimerReset(pageState); // 失败还原状态
                                Toast.ShowFail(error.Message);
                            }
                        });
                    }
                    break;
                case UILoginPageState.PhoneLoginPage:
                case UILoginPageState.EmailLoginPage:
                    {
                        UIService.Login.TimerSending(pageState); // 显示发送中状态

                        CodeCategory category = Config.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
                        BaseBridgeService.SendVerificationCode(account, CodeAction.Login, category, (data, error) =>
                        {
                            if (error == null)
                            {
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                UIService.Login.TimerReset(pageState); // 失败还原状态
                                Toast.ShowFail(error.Message);
                            }
                        });
                    }
                    break;
                default:
                    // 不发送处理
                    break;
            }
        }

        public void OnActivationCodeCommit(string code)
        {
            if (LoginBridgeService.GetCurrentAccessToken() != null)
            {
                Loader.ShowIndicator();

                AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

                LoginBridgeService.ActivationCodeCommit(accessToken.Value, code, (limitResult, error) =>
                {
                    Loader.HideIndicator();
                    LimitResultHandler(limitResult);
                });
            }
            else
            {
                Toast.ShowFail("当前未登录账号");
            }
        }

        public void OnRealnameInfoCommit(string realname, string cardID)
        {
            if (LoginBridgeService.GetCurrentAccessToken() != null)
            {
                Loader.ShowIndicator();

                AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

                LoginBridgeService.RealnameInfoCommit(accessToken.Value, realname, cardID, (limitResult, error) =>
                {
                    Loader.HideIndicator();
                    LimitResultHandler(limitResult);
                });
            }
            else
            {
                Toast.ShowFail("当前未登录账号");
            }
        }

        public void OnClickContactUS()
        {
            Toast.Show("功能开发中");
            BaseBridgeService.ContactUS();
        }

        public void OnReCallDelete()
        {
            if (FunnyDataStore.HasToken)
            {
                Loader.ShowIndicator();

                AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

                LoginBridgeService.RecallAccountDelete(accessToken.Value, (_, error) =>
                {
                    if (error == null)
                    {
                        VerifyLimit(accessToken);
                    }
                    else
                    {
                        Loader.HideIndicator();
                        Toast.ShowFail(error.Message);
                    }
                });
            }
            else
            {
                Toast.ShowFail("当前未登录账号");
            }
        }
    }

    #endregion

}

