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

        public void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler)
        {
            // TODO: 请求 /v1/account/oauth 接口数据
            Service.Call("FetchBindInfo", new AndroidCallBack<BindInfo>(handler));
        }

        public void Binding(IBindable bindable, ServiceCompletedHandler<VoidObject> handler)
        {
            if (bindable is EmailBindable model)
            {
                // 绑定邮箱
                Service.Call("BindWithEmail", model.Email, model.Password, model.Code, new AndroidCallBack<VoidObject>(handler));
            }
            else
            {
                // 绑定到第三方渠道
                // Flag = google, facebook, twitter 等第三方名称
                Service.Call("BindWithProvider", bindable.Flag, new AndroidCallBack<VoidObject>(handler));
            }
        }

        public Promise<BindInfo> FetchBindInfo()
        {
            return new Promise<BindInfo>((resolve, reject) =>
            {
                Service.Call("FetchBindInfo", new AndroidCallBack<BindInfo>(resolve, reject));
            });
        }
    }
}

#endif