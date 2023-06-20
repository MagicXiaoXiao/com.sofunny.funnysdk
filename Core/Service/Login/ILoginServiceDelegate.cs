using System;

namespace SoFunny.FunnySDK
{
    /// <summary>
    /// 登录服务代理接口
    /// </summary>
    public interface ILoginServiceDelegate
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="accessToken"></param>
        void OnLoginSuccess(AccessToken accessToken);
        /// <summary>
        /// 登录被用户取消
        /// </summary>
        void OnLoginCancel();
        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="error"></param>
        void OnLoginFailure(ServiceError error);
    }
}

