using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal static class IosHelper
    {

        internal static void HandlerServiceCallback<T>(bool success, string json, ServiceCompletedHandler<T> handler)
        {
            if (success)
            {
                var jsonObject = JsonConvert.DeserializeObject<T>(json);
                handler.Invoke(jsonObject, null);
            }
            else
            {
                var errorObject = JsonConvert.DeserializeObject<ServiceError>(json);
                handler.Invoke(default, errorObject);
            }
        }

    }
}

