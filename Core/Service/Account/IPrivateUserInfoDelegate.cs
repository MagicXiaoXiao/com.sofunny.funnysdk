using System;

namespace SoFunny.FunnySDK
{
    public interface IPrivateUserInfoDelegate
    {
        /// <summary>
        /// 用户同意授权
        /// </summary>
        /// <param name="userInfo"></param>
        void OnConsentAuthPrivateInfo(UserPrivateInfo userInfo);

        /// <summary>
        /// 用户跳过了本次授权
        /// </summary>
        void OnNextTime();

        /// <summary>
        /// 隐私授权服务未开启（平台未开启该服务）
        /// </summary>
        void OnUnenabledService();
    }
}

