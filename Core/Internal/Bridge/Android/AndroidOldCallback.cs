#if UNITY_ANDROID
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidOldCallback<T> : AndroidJavaProxy
    {
        private SynchronizationContext OriginalContext;
        private ServiceCompletedHandler<T> handler;

        internal AndroidOldCallback(ServiceCompletedHandler<T> handler) : base("com.xmfunny.funnysdk.unitywrapper.listener.FunnySdkCallback")
        {
            OriginalContext = SynchronizationContext.Current;
            this.handler = handler;
        }

        internal void OnComplete(string taskID, string jsonResult)
        {
            if (!string.IsNullOrEmpty(jsonResult))
            {
                OriginalContext.Post(_ =>
                {
                    var result = JsonConvert.DeserializeObject<NativeResult>(jsonResult);

                    if (result.success)
                    {
                        var value = result.TryGetValue<T>();
                        handler.Invoke(value, null);
                    }
                    else
                    {
                        var exception = JsonUtility.FromJson<ServiceError>(result.jsonValue);
                        handler.Invoke(default, exception);
                    }
                }, null);
            }
        }
    }
}

#endif