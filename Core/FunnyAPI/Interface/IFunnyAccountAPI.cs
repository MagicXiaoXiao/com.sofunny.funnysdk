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
        /// 登录账号 (发起登录流程，成功是将会触发 OnLoginEvents 事件)
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void Login(ILoginServiceDelegate serviceDelegate);

        /// <summary>
        /// 登出账号 (调用后会触发 OnLogoutEvents 事件)
        /// </summary>
        void Logout();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetUserProfile(IUserServiceDelegate serviceDelegate);

        /// <summary>
        /// 获取用户授权的隐私信息 (性别、生日等)
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate);
    }
}

