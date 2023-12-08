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

        public Promise Binding(IBindable bindable)
        {
            return new Promise((resolve, reject) =>
            {
                if (bindable is EmailBindable emailModel)
                {
                    FSDKCallAndBack.Builder("BindEmail")
                                   .Add("email", emailModel.Email)
                                   .Add("password", emailModel.Password)
                                   .Add("code", emailModel.Code)
                                   .Then(resolve)
                                   .Catch(reject)
                                   .Invoke();
                }
                else if (bindable is PhoneBindable phoneModel)
                {
                    FSDKCallAndBack.Builder("BindPhone")
                                   .Add("phone", phoneModel.PhoneNumber)
                                   .Add("code", phoneModel.Code)
                                   .Then(resolve)
                                   .Catch(reject)
                                   .Invoke();
                }
                else
                {
                    FSDKCallAndBack.Builder("BindProvider")
                                   .Add("provider", bindable.Flag)
                                   .Then(resolve)
                                   .Catch(reject)
                                   .Invoke();
                }
            });
        }

        public Promise<LoginResult> ForedBind(IBindable bindable, string bindCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                if (bindable is PhoneBindable phoneModel)
                {
                    FSDKCallAndBack.Builder("ForedBindPhone")
                                   .Add("phone", phoneModel.PhoneNumber)
                                   .Add("code", phoneModel.Code)
                                   .Add("bindCode", bindCode)
                                   .Then(resolve)
                                   .Catch(reject)
                                   .Invoke();
                }
                else
                {
                    Logger.LogWarning("暂无相关强制绑定项");
                    reject(ServiceError.Make(ServiceErrorType.UnknownError));
                }
            });
        }
    }
}

#endif