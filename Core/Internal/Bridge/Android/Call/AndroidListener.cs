#if UNITY_ANDROID
using System;
using System.Threading;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidListener : AndroidJavaProxy
    {
        SynchronizationContext OriginalContext;

        internal AndroidListener() : base("com.xmfunny.funnysdk.unitywrapper.listener.FunnySdkListener")
        {
            OriginalContext = SynchronizationContext.Current;
        }

        internal void Post(string identifier)
        {
            OriginalContext.Post(_ =>
            {
                BridgeNotificationCenter.Default.Post(identifier);
            }, null);

        }

        internal void Post(string identifier, string jsonValue)
        {
            OriginalContext.Post(_ =>
            {
                BridgeNotificationCenter.Default.Post(identifier, BridgeValue.Create(jsonValue));

            }, null);

        }

    }
}
#endif
