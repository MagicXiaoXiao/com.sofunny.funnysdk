using System;
using SoFunny.FunnySDK.UIModule;
using SoFunny.FunnySDK.Internal;
using Logger = SoFunny.FunnySDK.Internal.Logger;

namespace SoFunny.FunnySDK
{
    internal partial class FunnyLoginService : IFunnyLoginAPI
    {
        private SDKConfig Config;
        private IBridgeServiceLogin LoginBridgeService;
        private IBridgeServiceBase BaseBridgeService;

        internal FunnyLoginService(SDKConfig config, IBridgeServiceBase baseBridgeService, IBridgeServiceLogin loginBridgeService)
        {
            Config = config;
            BaseBridgeService = baseBridgeService;
            LoginBridgeService = loginBridgeService;

        }

        public void StartFlow()
        {
            UIService.Login.Open(this);
        }
    }

    internal partial class FunnyLoginService : ILoginViewEvent
    {
        public void OnClickPriacyProtocol()
        {
            Logger.Log("点击了隐私协议");
        }

        public void OnClickUserAgreenment()
        {
            Logger.Log("点击了用户协议");
        }

        public void OnCloseView(UILoginPageState pageState)
        {
            Logger.Log("关闭了登录页" + pageState);
        }

        public void OnLoginWithCode(string account, string code)
        {
            Logger.Log($"发起了验证码登录 - {account} - {code}");

            LoginBridgeService.LoginWithCode(account, code, (token, error) =>
            {
                if (error == null)
                {
                    Toast.ShowSuccess("登录成功");
                    // 后续逻辑
                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });

        }

        public void OnLoginWithPassword(string account, string password)
        {
            Logger.Log($"发起了账号密码登录 - {account} - {password}");

            LoginBridgeService.LoginWithPassword(account, password, (token, error) =>
            {
                if (error == null)
                {

                }
                else
                {
                    Toast.ShowFail(error.Message);
                }
            });
        }

        public void OnLoginWithProvider()
        {
            Logger.Log("发起了第三方登录");
        }

        public void OnRegisterAccount(string account, string pwd, string code)
        {
            Logger.Log($"发起账号注册- {account} - {pwd} - {code}");
            Loader.ShowIndicator();
            LoginBridgeService.RegisterAccount(account, pwd, code, (ssoToken, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    // 成功处理
                    Toast.ShowSuccess("注册成功");
                    Logger.Log("注册成功 - " + ssoToken);
                }
                else
                {
                    Toast.ShowFail(error.Message);
                    Logger.Log($"注册失败 - {error.Message}");
                }
            });

        }

        public void OnRetrievePassword(string account, string newPwd, string code)
        {
            Logger.Log($"找回密码- {account} - {newPwd} - {code}");
            Loader.ShowIndicator();
            LoginBridgeService.RetrievePassword(account, newPwd, code, (none, error) =>
            {
                Loader.HideIndicator();
                if (error == null)
                {
                    Toast.ShowSuccess("修改成功");
                    Logger.Log("修改密码成功");
                }
                else
                {
                    Toast.ShowFail(error.Message);
                    Logger.Log($"修改密码失败 - {error.Message}");
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

