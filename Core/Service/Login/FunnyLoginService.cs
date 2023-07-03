using System;
using UnityEngine;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyLoginService
    {
        private readonly FunnySDKConfig Config;
        private readonly LoginTrack Analysis;

        private readonly IBridgeServiceLogin LoginBridgeService;
        private readonly IBridgeServiceBase BaseBridgeService;
        private ILoginServiceDelegate LoginDelegate;
        private bool StartFlag = false;

        internal FunnyLoginService(
            FunnySDKConfig config,
            IBridgeServiceBase baseBridgeService,
            IBridgeServiceLogin loginBridgeService,
            IBridgeServiceTrack trackService
            )
        {
            Config = config;
            Analysis = new LoginTrack(trackService);
            BaseBridgeService = baseBridgeService;
            LoginBridgeService = loginBridgeService;
        }

        internal void Logout()
        {
            LoginBridgeService.Logout();
        }

        internal void StartLogin(ILoginServiceDelegate serviceDelegate)
        {
            if (StartFlag)
            {
                Logger.LogWarning("已有登录正在进行中，请等待完成。");
                return;
            }

            StartFlag = true;

            Analysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

#if UNITY_ANDROID && !UNITY_EDITOR
            // Android 平台暂不显示指示器 UI
#else
            Loader.ShowIndicator();
#endif
            // 设置代理对象
            LoginDelegate = serviceDelegate;

            BaseBridgeService.GetAppInfo((appConfig, error) =>
            {
                if (error == null)
                {
                    UIService.Login.SetupLoginConfig(this, appConfig.GetLoginProviders());

                    AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

                    if (accessToken != null)
                    {
                        // 已登录，验证 Token
                        VerifyLimit(accessToken, true);
                    }
                    else
                    {
                        Loader.HideIndicator();
                        UIService.Login.Open();
                    }
                }
                else
                {
                    Analysis.SdkLoginResultFailure(false, error);

                    Loader.HideIndicator();

                    StartFlag = false;
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

        public void OnSwitchOtherAccount()
        {
            Analysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录账号被限制"));
            LoginBridgeService.Logout();

            Analysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);
            UIService.Login.JumpTo(UILoginPageState.LoginSelectPage);
        }

        public void OnOpenView(UILoginPageState current, UILoginPageState prev)
        {
            Logger.Log($"打开了登录页: 当前页面 - {current}, 上一个页面 - {prev}");
            Analysis.SdkPageLoad((int)current, (int)prev);
        }

        public void OnCloseView(UILoginPageState pageState)
        {
            Logger.Log("关闭了登录页" + pageState);
            Analysis.SdkPageClose((int)pageState);
            Analysis.SdkLoginResultFailure(false, new ServiceError(-1, "登录被取消"));

            StartFlag = false;
            LoginDelegate?.OnLoginCancel();
            LoginDelegate = null;
        }

        public void OnLoginWithCode(string account, string code)
        {
            Logger.Log($"发起了验证码登录 - {account} - {code}");
            Loader.ShowIndicator();

            Analysis.SetLoginFrom(3);
            Analysis.SetLoginWay(Config.IsMainland ? 103 : 101);

            LoginBridgeService.LoginWithCode(account, code, (token, error) =>
            {
                if (error == null)
                {
                    Analysis.SdkVerifyCodeSuccess(1, 202);
                    Analysis.SdkStartLoginSuccess(false, true);
                    // 数据存储
                    FunnyDataStore.UpdateToken(token);
                    // 验证 Token
                    VerifyLimit(token);
                }
                else
                {
                    Analysis.SdkVerifyCodeFailure(1, 202, error);
                    Analysis.SdkStartLoginFailure(false, true, error);

                    Loader.HideIndicator();
                    Toast.ShowFail(error.Message);
                }
            });

        }

        public void OnLoginWithPassword(string account, string password)
        {
            Logger.Log($"发起了账号密码登录 - {account} - {password}");
            Loader.ShowIndicator();

            Analysis.SetLoginFrom(1);
            Analysis.SetLoginWay(Config.IsMainland ? 103 : 101);

            LoginBridgeService.LoginWithPassword(account, password, (token, error) =>
            {
                if (error == null)
                {
                    Analysis.SdkStartLoginSuccess(false, true);
                    // 数据存储
                    FunnyDataStore.UpdateToken(token);
                    // 验证 Token
                    VerifyLimit(token);
                }
                else
                {
                    Analysis.SdkStartLoginFailure(false, true, error);

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
                    if (auto)
                    {
                        Analysis.SdkStartLoginSuccess(auto, true);
                    }

                    LimitResultHandler(limitStatus);
                }
                else
                {
                    if (error.Error == ServiceErrorType.InvalidAccessToken) // Token 过期处理
                    {
                        Analysis.SdkLoginResultFailure(token.NewUser, error);

                        Toast.ShowFail("授权已过期，请重新登录");

                        // 重新发起流程
                        Analysis.SdkPageOpen((int)UILoginPageState.LoginSelectPage);

                        UIService.Login.Open();
                        return;
                    }

                    Toast.ShowFail(error.Message);

                    if (auto)
                    {
                        Analysis.SdkStartLoginFailure(auto, true, error);

                        StartFlag = false;
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

                        Analysis.SdkLoginResultSuccess(token.NewUser);

                        StartFlag = false;
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
                    Analysis.SdkVerifyCodeFailure(2, 402, new ServiceError(limitStatus.StatusCode, "无效邀请码"));
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

            if (provider == LoginProvider.Guest)
            {
                Analysis.SetLoginFrom(5);
            }
            else
            {
                Analysis.SetLoginFrom(4);
            }

            Analysis.SetLoginWay((int)provider);

            LoginBridgeService.LoginWithProvider(provider, (accessToken, error) =>
            {

                Loader.HideIndicator();
                if (error == null)
                {
                    Analysis.SdkStartLoginSuccess(false, true);

                    VerifyLimit(accessToken);
                }
                else
                {
                    if (error.Code == -3000)// 暂定取消
                    {
                        Analysis.SdkTPAuthCancel();
                    }
                    else if (error.Code == -3001)// 暂定失败
                    {
                        Analysis.SdkTPAuthFailure(error);
                    }
                    else
                    {
                        Analysis.SdkStartLoginFailure(false, true, error);
                    }

                    Toast.ShowFail(error.Message);
                }
            });
        }

        public void OnRegisterAccount(string account, string pwd, string code)
        {
            Loader.ShowIndicator();

            Analysis.SetLoginFrom(2);

            LoginBridgeService.RegisterAccount(account, pwd, code, (accessToken, error) =>
            {
                if (error == null)
                {
                    Analysis.SdkVerifyCodeSuccess(1, 203);
                    // 数据存储
                    FunnyDataStore.UpdateToken(accessToken);
                    // 验证 Token
                    VerifyLimit(accessToken);
                }
                else
                {
                    Analysis.SdkVerifyCodeFailure(1, 203, error);
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
                    Analysis.SdkVerifyCodeSuccess(1, 301);

                    Toast.ShowSuccess("修改成功");
                    // 跳转页面
                    UIService.Login.JumpTo(UILoginPageState.PwdLoginPage);
                }
                else
                {
                    Analysis.SdkVerifyCodeFailure(1, 301, error);

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
                                Analysis.SdkSendCodeSuccess((int)pageState);
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                Analysis.SdkSendCodeFailure((int)pageState, error);
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
                                Analysis.SdkSendCodeSuccess((int)pageState);
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                Analysis.SdkSendCodeFailure((int)pageState, error);
                                UIService.Login.TimerReset(pageState); // 失败还原状态
                                Toast.ShowFail(error.Message);
                            }
                        });
                    }
                    break;
                case UILoginPageState.CodeLoginPage:
                    {
                        UIService.Login.TimerSending(pageState); // 显示发送中状态

                        CodeCategory category = Config.IsMainland ? CodeCategory.Phone : CodeCategory.Email;
                        BaseBridgeService.SendVerificationCode(account, CodeAction.Login, category, (data, error) =>
                        {
                            if (error == null)
                            {
                                Analysis.SdkSendCodeSuccess((int)pageState);
                                UIService.Login.TimerStart(pageState); // 成功开始倒计时
                            }
                            else
                            {
                                Analysis.SdkSendCodeFailure((int)pageState, error);
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
            AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

            if (accessToken is null)
            {
                Toast.ShowFail("当前未登录账号");
                return;
            }

            Loader.ShowIndicator();

            LoginBridgeService.ActivationCodeCommit(accessToken.Value, code, (limitResult, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    LimitResultHandler(limitResult);
                }
                else
                {
                    Analysis.SdkVerifyCodeFailure(2, 402, error);
                    Toast.ShowFail(error.Message);
                }
            });
        }

        public void OnRealnameInfoCommit(string realname, string cardID)
        {
            if (LoginBridgeService.GetCurrentAccessToken() is null)
            {
                Toast.ShowFail("当前未登录账号");
                return;
            }

            Loader.ShowIndicator();

            AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

            LoginBridgeService.RealnameInfoCommit(accessToken.Value, realname, cardID, (limitResult, error) =>
            {
                Loader.HideIndicator();
                LimitResultHandler(limitResult);
            });
        }

        public void OnClickContactUS()
        {
            Analysis.SdkPageOpen(702);

            BaseBridgeService.ContactUS();
        }

        public void OnReCallDelete()
        {
            Alert.Show("提示", "您将撤回账号永久删除申请，解除账号锁定状态",
                new AlertActionItem("取消"),
                new AlertActionItem("确定", () =>
                {
                    ReCallDeleteHandler();
                }));
        }

        private void ReCallDeleteHandler()
        {
            AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

            if (accessToken is null)
            {
                Toast.ShowFail("当前未登录账号");
                return;
            }

            Loader.ShowIndicator();

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
    }

    #endregion

}

