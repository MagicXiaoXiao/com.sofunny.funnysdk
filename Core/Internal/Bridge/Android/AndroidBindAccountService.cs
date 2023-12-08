#if UNITY_ANDROID

using System;
using UnityEngine;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidBindAccountService : IBridgeServiceBind
    {
        private readonly AndroidJavaObject Service;

        internal AndroidBindAccountService()
        {
            Service = new AndroidJavaObject("com.xmfunny.funnysdk.unitywrapper.internal.unity.FunnySdkWrapper4Unity");
        }

        public Promise Binding(IBindable bindable)
        {
            return new Promise((resolve, reject) =>
            {
                if (bindable is EmailBindable model)
                {
                    // 绑定邮箱
                    Service.Call("BindWithEmail", model.Email, model.Password, model.Code, new AndroidCallBack(resolve, reject));
                }
                else
                {
                    // 绑定到第三方渠道
                    // Flag = google, facebook, twitter 等第三方名称
                    Service.Call("BindWithProvider", bindable.Flag, new AndroidCallBack(resolve, reject));
                }
            });
        }

        public Promise<BindInfo> FetchBindInfo()
        {
            return new Promise<BindInfo>((resolve, reject) =>
            {
                Service.Call("FetchBindInfo", new AndroidCallBack<BindInfo>(resolve, reject));
            });
        }

        public Promise<LoginResult> ForedBind(IBindable bindable, string bindCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                if (bindable is PhoneBindable phoneModel)
                {
                    // Tips: 根据手机号、验证码、绑定码。请求 /account/third_party_bind_account 接口。
                    //       成功时，SDK 需将 SSO Token 内部进行存储，后续会进行 VerifyLimit 流程。
                    Service.Call("ForedBindPhone", phoneModel.PhoneNumber, phoneModel.Code, bindCode, new AndroidCallBack<LoginResult>(resolve, reject));
                }
                else
                {
                    reject?.Invoke(new ServiceError(-1, "暂不支持非手机号码外的强制绑定逻辑"));
                }
            });
        }
    }
}

#endif