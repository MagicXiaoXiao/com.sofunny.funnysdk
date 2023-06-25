using System;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// Android 回调接口处理对象
    /// </summary>
    internal class AndroidCallBack<T> : AndroidJavaProxy
    {
        private readonly ServiceCompletedHandler<T> CallbackHandler;
        private readonly SynchronizationContext OriginalContext;

        internal AndroidCallBack(ServiceCompletedHandler<T> handler) : base("com.xmfunny.funnysdk.unitywrapper.internal.unity.FunnyUnityCallBack")
        {
            OriginalContext = SynchronizationContext.Current;

            CallbackHandler = handler;
        }

        internal void OnSuccessHandler(string jsonModel)
        {
            Logger.Log($"AndroidBridgeCallback - Success - {jsonModel}");

            try
            {
                var model = JsonConvert.DeserializeObject<T>(jsonModel);

                OriginalContext.Post(_ =>
                 {
                     CallbackHandler?.Invoke(model, null);

                 }, null);
            }
            catch (JsonException ex)
            {
                Logger.LogError("AndroidBridgeCallback 回调数据解析出错 - " + ex.Message);

                OriginalContext.Post(_ =>
                {
                    CallbackHandler?.Invoke(default, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));

                }, null);
            }
        }

        internal void OnFailureHandler(int code, string message)
        {
            Logger.LogError($"AndroidBridgeCallback - Failure - [code = {code}, message = {message}]");

            OriginalContext.Post(_ =>
            {
                CallbackHandler?.Invoke(default, new ServiceError(code, message));
            }, null);
        }
    }
}

