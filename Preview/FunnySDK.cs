using System;
using UnityEngine;
using System.Threading.Tasks;

namespace SoFunny.FunnySDKPreview
{

    public sealed partial class FunnySDK
    {
        private FunnySDK() { }
        /// <summary>
        /// FunnySDK 初始化方法
        /// </summary>
        [Obsolete("该方法已废弃请使用 Initialize 方法")]
        public static void InitializeSDK(string appID)
        {
            Initialize();
        }

        /// <summary>
        /// 初始化 SDK 方法，请确保在 Funny 面板上已配置相关参数
        /// </summary>
        public static void Initialize()
        {
            Native.GetInstance().SetupSDK();
        }

        /// <summary>
        /// 发起登录流程，会打开对应登录 UI 界面
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FunnySDKException"></exception>
        /// <exception cref="LoginCancelledException"></exception>
        public static Task<AccessToken> Login()
        {
            return Native.GetInstance().Login((int)LoginType.SoFunny);
        }

        /// <summary>
        /// 登出当前账户
        /// </summary>
        /// <returns></returns>
        public static Task<bool> Logout()
        {
            return Native.GetInstance().Logout();
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns>UserProfile</returns>
        /// <exception cref="NotLoggedInException"></exception>
        /// <exception cref="AccessTokenInvalidException"></exception>
        public static Task<UserProfile> GetProfile()
        {
            return Native.GetInstance().GetProfile();
        }

        /// <summary>
        /// 获取用户隐私信息 <br/>
        /// 如用户未填写信息，则会展示填写 UI 界面
        /// </summary>
        /// <returns>PrivacyProfile</returns>
        /// <exception cref="FunnySDKException"></exception>
        /// <exception cref="PrivacyProfileCancelledException"></exception>
        /// <exception cref="PrivacyProfileDisableException"></exception>
        /// <exception cref="PrivacyProfilePlatformException"></exception>
        public static Task<PrivacyProfile> AuthPrivacyProfile()
        {
            return Native.GetInstance().AuthPrivacyProfile();
        }

        /// <summary>
        /// 获取当前已登录用户 AccessToken
        /// </summary>
        /// <returns>AccessToken Or Null</returns>
        public static AccessToken GetCurrentAccessToken()
        {
            return Native.GetInstance().GetCurrentAccessToken();
        }

        /// <summary>
        /// 打开个人中心界面
        /// </summary>
        public static void OpenUserCenterUI()
        {
            Native.GetInstance().OpenUserCenter();
        }

        /// <summary>
        /// 关闭个人中心界面
        /// </summary>
        public static void CloseUserCenterUI()
        {
            Native.GetInstance().CloseUserCenter();
        }

        /// <summary>
        /// 打开公告界面
        /// </summary>
        /// <param name="style">界面样式</param>
        public static void OpenBillboardUI(BillboardStyle style)
        {
            Native.GetInstance().OpenBillboard();
        }

        /// <summary>
        /// 是否含有公告
        /// </summary>
        /// <returns></returns>
        public static Task<bool> AnyBillMessage()
        {
            return Native.GetInstance().AnyBillMessage();
        }

        /// <summary>
        /// 打开反馈界面
        /// </summary>
        /// <param name="playerID">玩家编号</param>
        public static void OpenFeedbackUI(string playerID = "")
        {
            Native.GetInstance().OpenFeedback(playerID);
        }
        /// <summary>
        /// 打开用户协议界面
        /// </summary>
        /// <returns>void</returns>
        public static void ShowProtocol()
        {
            Native.GetInstance().ShowProtocol();
        }
    }

    public partial class FunnySDK
    {
        public delegate void FunnySDKAction();
        public delegate void FunnySDKAction<in T>(T obj);

        /// <summary>
        /// FunnySDK 用户登录后会调用该事件
        /// </summary>
        public static event FunnySDKAction<AccessToken> OnLoginEvent;
        /// <summary>
        /// FunnySDK 用户登出后会调用该事件
        /// </summary>
        public static event FunnySDKAction OnLogoutEvent;
        /// <summary>
        /// 用户中心被打开后会调用该事件
        /// </summary>
        public static event FunnySDKAction OnOpenUserCenterEvent;
        /// <summary>
        /// 用户中心被关闭后会调用该事件
        /// </summary>
        public static event FunnySDKAction OnCloseUserCenterEvent;
        /// <summary>
        /// 游客账号完成绑定后会调用该事件
        /// </summary>
        public static event FunnySDKAction<AccessToken> OnGuestDidBindEvent;
        /// <summary>
        /// 当前账号切换到新账号后会调用该事件
        /// </summary>
        public static event FunnySDKAction<AccessToken> OnSwitchAccountEvent;
        /// <summary>
        /// 公告面板被打开事件
        /// </summary>
        public static event FunnySDKAction OnOpenBillboardEvent;
        /// <summary>
        /// 公告面板被关闭事件
        /// </summary>
        public static event FunnySDKAction OnCloseBillboardEvent;
        /// <summary>
        /// 反馈面板被打开事件
        /// </summary>
        public static event FunnySDKAction OnOpenFeedbackEvent;
        /// <summary>
        /// 反馈面板被关闭事件
        /// </summary>
        public static event FunnySDKAction OnCloseFeedbackEvent;
        /// <summary>
        /// 用户隐私协议同意事件
        /// </summary>
        public static event FunnySDKAction OnConfirmProtocolEvent;

        internal static void OnNativeListener(string identifier, string value = null)
        {
            switch (identifier)
            {
                case "event.login":
                    if (!string.IsNullOrEmpty(value))
                    {
                        var result = JsonUtility.FromJson<AccessToken>(value);
                        OnLoginEvent?.Invoke(result);
                    }
                    break;
                case "event.logout":
                    OnLogoutEvent?.Invoke();
                    break;
                case "event.open.userCenter":
                    OnOpenUserCenterEvent?.Invoke();
                    break;
                case "event.close.userCenter":
                    OnCloseUserCenterEvent?.Invoke();
                    break;
                case "event.bind.finish":
                    if (!string.IsNullOrEmpty(value))
                    {
                        var result = JsonUtility.FromJson<AccessToken>(value);
                        OnGuestDidBindEvent?.Invoke(result);
                    }
                    break;
                case "event.switch.account":
                    if (!string.IsNullOrEmpty(value))
                    {
                        var result = JsonUtility.FromJson<AccessToken>(value);
                        OnSwitchAccountEvent.Invoke(result);
                    }
                    break;
                case "event.open.billboard":
                    OnOpenBillboardEvent?.Invoke();
                    break;
                case "event.close.billboard":
                    OnCloseBillboardEvent?.Invoke();
                    break;
                case "event.open.feedback":
                    OnOpenFeedbackEvent?.Invoke();
                    break;
                case "event.close.feedback":
                    OnCloseFeedbackEvent?.Invoke();
                    break;
                case "event.open.protocol":
                    OnConfirmProtocolEvent?.Invoke();
                    break;
                default:
                    Debug.Log("[FunnySDK]: 暂未含有该事件处理: " + identifier);
                    break;
            }
        }
    }

}