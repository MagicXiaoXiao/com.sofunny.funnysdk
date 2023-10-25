#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCBindAccountService : IBridgeServiceBind
    {
        internal PCBindAccountService()
        {
        }

        public void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler)
        {
            SSOToken token = FunnyDataStore.GetCurrentToken();

            if (token is null)
            {
                handler?.Invoke(null, ServiceError.Make(ServiceErrorType.NoLoginError));
                return;
            }

            Network.Send(new GetAccountBindInfoRequest(token.Value), (jsonValue, error) =>
            {
                if (error is null)
                {
                    BindInfo bindInfo = JsonConvert.DeserializeObject<BindInfo>(jsonValue);

                    handler?.Invoke(bindInfo, null);
                }
                else
                {
                    handler?.Invoke(null, error);
                }
            });
        }

        public void Binding(IBindable bindable, ServiceCompletedHandler<VoidObject> handler)
        {
            handler?.Invoke(null, new ServiceError(-1, "PC 版本暂不支持该功能"));
        }
    }
}

#endif