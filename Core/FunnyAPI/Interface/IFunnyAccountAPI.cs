using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyAccountAPI
    {
        /// <summary>
        /// 登录账号（发起登录流程）
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void Login(ILoginServiceDelegate serviceDelegate);

        /// <summary>
        /// 登出账号
        /// </summary>
        void Logout();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetUserProfile(IUserServiceDelegate serviceDelegate);

        /// <summary>
        /// 获取用户授权的隐私信息（性别、生日等）
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetPrivateUserInfo(IPrivateUserInfoDelegate serviceDelegate);
    }
}

