using System;
using UnityEngine;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyLoginService : IFunnyLoginAPI
    {
        private readonly SDKConfig Config;
        private readonly IBridgeServiceLogin LoginBridgeService;
        private readonly IBridgeServiceBase BaseBridgeService;
        private ILoginServiceDelegate LoginDelegate;

        internal FunnyLoginService(SDKConfig config, IBridgeServiceBase baseBridgeService, IBridgeServiceLogin loginBridgeService)
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

        public void Logout()
        {
            LoginBridgeService.Logout();
        }

        public void StartFlow(ILoginServiceDelegate serviceDelegate)
        {
            Loader.ShowIndicator();

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
                    VerifyLimit(token);
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
        private void VerifyLimit(AccessToken token)
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
                    // UI 展示逻辑
                    Toast.ShowSuccess("登录成功");
                    // 关闭 UI 以及后续逻辑
                    UIService.Login.CloseView();

                    AccessToken token = LoginBridgeService.GetCurrentAccessToken();
                    LoginDelegate?.OnLoginSuccess(token);
                    LoginDelegate = null;
                    break;
                case LimitStatus.StatusType.AccountBannedFailed:
                    // 账号已被封禁处理
                    UIService.Login.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
                case LimitStatus.StatusType.AllowFailed:
                    // IP 限制页面
                    UIService.Login.JumpTo(UILoginPageState.LoginLimitPage);
                    break;
                case LimitStatus.StatusType.AccountInCooldownFailed:
                    // 账号冷静期页面
                    UIService.Login.JumpTo(UILoginPageState.CoolDownTipsPage);
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
                                Toast.ShowFail("发送失败");
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
                                Toast.ShowFail("发送失败");
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
                                Toast.ShowFail("发送失败");
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

}

