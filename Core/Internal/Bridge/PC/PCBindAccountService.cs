#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using Newtonsoft.Json;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCBindAccountService : IBridgeServiceBind
    {
        internal PCBindAccountService()
        {
        }

        public Promise Binding(IBindable bindable)
        {
            return new Promise((resolve, reject) =>
            {
                reject?.Invoke(new ServiceError(-1, "PC 版本暂不支持该功能"));
            });
        }

        public Promise<BindInfo> FetchBindInfo()
        {
            return new Promise<BindInfo>((resolve, reject) =>
            {
                SSOToken token = FunnyDataStore.Current;

                if (token is null)
                {
                    reject(ServiceError.Make(ServiceErrorType.NoLoginError));
                    return;
                }

                Network.Send(new GetAccountBindInfoRequest(token.Value), (jsonValue, error) =>
                {
                    if (error is null)
                    {
                        BindInfo bindInfo = JsonConvert.DeserializeObject<BindInfo>(jsonValue);

                        resolve(bindInfo);
                    }
                    else
                    {
                        reject(error);
                    }
                });

            });
        }

        public Promise<LoginResult> ForedBind(IBindable bindable, string bindCode)
        {
            return new Promise<LoginResult>((resolve, reject) =>
            {
                reject?.Invoke(new ServiceError(-1, "PC 版本暂不支持该功能"));
            });
        }
    }
}

#endif