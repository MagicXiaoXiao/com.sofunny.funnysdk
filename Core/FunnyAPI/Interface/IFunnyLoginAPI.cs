using System;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK
{
    public interface IFunnyLoginAPI
    {
        /// <summary>
        /// 发起登录
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void StartFlow(ILoginServiceDelegate serviceDelegate);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="serviceDelegate"></param>
        void GetUserProfile(IUserServiceDelegate serviceDelegate);
        /// <summary>
        /// 登出当前账户
        /// </summary>
        void Logout();
    }
}

