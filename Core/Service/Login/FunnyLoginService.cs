using System;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;
using Logger = SoFunny.FunnySDK.Internal.Logger;

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
            FunnyDataStore.DeleteToken();
        }

        public void StartFlow(ILoginServiceDelegate serviceDelegate)
        {
            Loader.ShowIndicator();

            // 设置代理对象
            LoginDelegate = serviceDelegate;
            AccessToken accessToken = LoginBridgeService.GetCurrentAccessToken();

            if (accessToken != null)
            {
                // 已登录，验证 Token
                VerifyLimit(accessToken);
            }
            else
            {
                // 获取应用信息
                BaseBridgeService.GetAppInfo((appConfig, error) =>
                {
                    Loader.HideIndicator();

                    if (error == null)
                    {
                        UIService.Login.Open(this, appConfig.GetLoginProviders());
                    }
                    else
                    {
                        Toast.ShowFail(error.Message);
                        LoginDelegate?.OnLoginFailure(error);
                    }
                });
            }
        }
    }

    internal partial class FunnyLoginService : ILoginViewEvent
    {
        public void OnClickPriacyProtocol()
        {
            BaseBridgeService.OpenPriacyProtocol();
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
                    switch (limitStatus.Status)
                    {
                        case LimitStatus.StatusType.Success:
                            // TODO: 数据存储逻辑(后续补充)
                            FunnyDataStore.UpdateToken(token);
                            // UI 展示逻辑
                            Toast.ShowSuccess("登录成功");
                            // 关闭 UI 以及后续逻辑
                            UIService.Login.CloseView();
                            LoginDelegate?.OnLoginSuccess(token);
                            LoginDelegate = null;
                            break;
                        case LimitStatus.StatusType.AccountBannedFailed:
                            // 账号已被封禁处理
                            break;
                        default:
                            Toast.ShowFail("登录被限制");
                            break;
                    }
                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });
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
                    // 成功处理
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
    }

}

