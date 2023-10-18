using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyAccountAPI
    {
        /// <summary>
        /// 登录成功事件 (当有账号登录成功时会触发)
        /// </summary>
        event Action<AccessToken> OnLoginEvents;

        /// <summary>
        /// 登出事件 (当前账号登出时会触发)
        /// </summary>
        event Action OnLogoutEvents;

        /// <summary>
        /// 账号被切换后触发事件 (返回切换后的目标 AccessToken)
        /// </summary>
        event Action<AccessToken> OnSwitchAccountEvents;

        /// <summary>
        /// 获取当前账号 AccessToken (如未登录，则返回 null)
        /// </summary>
        /// <returns></returns>
        AccessToken GetCurrentAccessToken();

        /// <summary>
        /// 登录账号 (发起登录流程，成功是将会触发 OnLoginEvents 事件)
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void Login(ILoginServiceDelegate serviceDelegate);

        /// <summary>
        /// 登录账号 (发起登录流程，成功是将会触发 OnLoginEvents 事件)
        /// </summary>
        /// <param name="onSuccessHandler">成功处理</param>
        /// <param name="onFailureHandler">失败处理</param>
        /// <param name="onCancelHandler">取消处理</param>
        void Login(Action<AccessToken> onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler);

        /// <summary>
        /// 登出账号 (调用后会触发 OnLogoutEvents 事件)
        /// </summary>
        void Logout();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="serviceDelegate"></param>
        [Obsolete("当前方法后续将会移除，请使用 FetchUserProfile 方法", false)]
        void GetUserProfile(IUserServiceDelegate serviceDelegate);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="onSuccessHandler">成功处理</param>
        /// <param name="onFailureHandler">失败处理</param>
        [Obsolete("当前方法后续将会移除，请使用 FetchUserProfile 或者形参的 GetUserProfile 方法", false)]
        void GetUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler);

        /// <summary>
        /// 获取当前用户信息。如未登录，则返回 null
        /// </summary>
        /// <returns></returns>
        UserProfile GetUserProfile();

        /// <summary>
        /// 从远端拉取用户信息
        /// </summary>
        /// <param name="onSuccessHandler"></param>
        /// <param name="onFailureHandler"></param>
        void FetchUserProfile(Action<UserProfile> onSuccessHandler, Action<ServiceError> onFailureHandler);

        /// <summary>
        /// 获取用户授权的隐私信息 (性别、生日等)
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate);

    }
}

