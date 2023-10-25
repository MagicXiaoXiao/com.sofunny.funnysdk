#if UNITY_IOS
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKBindAccountService : IBridgeServiceBind
    {
        internal FSDKBindAccountService()
        {
        }

        public void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler)
        {
            // TODO: 暂不实现
            throw new NotImplementedException();
        }

        public void Binding(IBindable bindable, ServiceCompletedHandler<VoidObject> handler)
        {
            if (bindable is EmailBindable emailModel)
            {
                FSDKCallAndBack.Builder("BindEmail")
                               .Add("email", emailModel.Email)
                               .Add("password", emailModel.Password)
                               .Add("code", emailModel.Code)
                               .AddCallbackHandler((result, json) =>
                               {
                                   IosHelper.HandlerServiceCallback(result, json, handler);
                               })
                               .Invoke();
            }
            else
            {
                // TODO: 待补充绑定第三方逻辑
            }
        }


    }
}

#endif