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
        ///
        [Obsolete("该方法后续将会删除，请使用其他重载方法", false)]
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
        [Obsolete("当前方法后续将会移除，请使用 FetchUserProfile 或者 GetUserProfile() 方法", false)]
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
        ///
        [Obsolete("请使用 AuthPrivateUserInfo 方法将其替换，后续将会删除当前方法", false)]
        void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate);

        /// <summary>
        /// 获取用户授权的隐私信息 (性别、生日等)
        /// </summary>
        /// <param name="onSuccessAction">用户已授权</param>
        /// <param name="onCancelAction">用户已取消， bool 表示为隐私服务是否已开启</param>
        void AuthPrivateUserInfo(Action<UserPrivateInfo> onSuccessAction, Action<bool> onCancelAction);

        /// <summary>
        /// 绑定当前账号至目标平台渠道
        /// </summary>
        /// <param name="type">绑定类型</param>
        /// <param name="onSuccessHandler">成功处理</param>
        /// <param name="onFailureHandler">失败处理</param>
        /// <param name="onCancelHandler">取消处理</param>
        void Bind(BindingType type, Action onSuccessHandler, Action<ServiceError> onFailureHandler, Action onCancelHandler);

        /// <summary>
        /// 获取当前账号绑定状态数据列表，如未登录则返回空数组
        /// </summary>
        /// <returns></returns>
        BindStatusItem[] GetBindStatus();
    }
}

