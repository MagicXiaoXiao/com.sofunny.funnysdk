#if UNITY_ANDROID
using System;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidCallBack : AndroidCallBack<VoidObject>
    {
        internal AndroidCallBack(Action onSuccessAction, Action<ServiceError> onFailureAction) : base((_) => { onSuccessAction?.Invoke(); }, onFailureAction) { }
    }

    /// <summary>
    /// Android 回调接口处理对象
    /// </summary>
    internal class AndroidCallBack<T> : AndroidJavaProxy
    {
        private readonly ServiceCompletedHandler<T> CallbackHandler;
        private readonly SynchronizationContext OriginalContext;

        private readonly Action<T> OnSuccess;
        private readonly Action<ServiceError> OnFailure;

        internal AndroidCallBack(ServiceCompletedHandler<T> handler) : base("com.xmfunny.funnysdk.unitywrapper.internal.unity.FunnyUnityCallBack")
        {
            OriginalContext = SynchronizationContext.Current;

            CallbackHandler = handler;
        }

        internal AndroidCallBack(Action<T> onSuccessAction, Action<ServiceError> onFailureAction) : base("com.xmfunny.funnysdk.unitywrapper.internal.unity.FunnyUnityCallBack")
        {
            OriginalContext = SynchronizationContext.Current;

            OnSuccess = onSuccessAction;
            OnFailure = onFailureAction;

            CallbackHandler = null;
        }

        internal void OnSuccessHandler(string jsonModel)
        {
            Logger.Log($"AndroidBridgeCallback - Success - {jsonModel}");

            // 部分业务逻辑本身不需要有结果值，故此处无需做非空判断逻辑

            try
            {

                var model = JsonConvert.DeserializeObject<T>(jsonModel);

                OriginalContext.Post(_ =>
                 {
                     OnSuccess?.Invoke(model);
                     CallbackHandler?.Invoke(model, null);

                 }, null);
            }
            catch (JsonException ex)
            {
                Logger.LogError("AndroidBridgeCallback 回调数据解析出错 - " + ex.Message);

                OriginalContext.Post(_ =>
                {
                    OnFailure?.Invoke(ServiceError.Make(ServiceErrorType.ProcessingDataFailed));
                    CallbackHandler?.Invoke(default, ServiceError.Make(ServiceErrorType.ProcessingDataFailed));

                }, null);
            }
        }

        internal void OnFailureHandler(int code, string message)
        {
            Logger.LogError($"AndroidBridgeCallback - Failure - [code = {code}, message = {message}]");

            OriginalContext.Post(_ =>
            {
                OnFailure?.Invoke(new ServiceError(code, message));
                CallbackHandler?.Invoke(default, new ServiceError(code, message));
            }, null);
        }
    }



}

#endif

