#if UNITY_IOS

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                var errorObj = JObject.Parse(json);
                int code = errorObj.Value<int>("code");
                string message = errorObj.Value<string>("message");

                var errorObject = new ServiceError(code, message);
                handler.Invoke(default, errorObject);
            }
        }

    }
}

#endif
