#if UNITY_IOS
using System;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKBindAccountService : IBridgeServiceBind
    {
        internal FSDKBindAccountService()
        {
        }

        public void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler)
        {
            FSDKCallAndBack.Builder("FetchBindInfo")
                           .AddCallbackHandler((result, json) =>
                           {
                               IosHelper.HandlerServiceCallback(result, json, handler);
                           })
                           .Invoke();
        }

        public Promise<BindInfo> FetchBindInfo()
        {
            return new Promise<BindInfo>((resolve, reject) =>
            {
                FSDKCallAndBack.Builder("FetchBindInfo")
                               .Then(resolve)
                               .Catch(reject)
                               .Invoke();
            });
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
                FSDKCallAndBack.Builder("BindProvider")
                               .Add("provider", bindable.Flag)
                               .AddCallbackHandler((result, json) =>
                               {
                                   IosHelper.HandlerServiceCallback(result, json, handler);
                               })
                               .Invoke();
            }
        }


    }
}

#endif