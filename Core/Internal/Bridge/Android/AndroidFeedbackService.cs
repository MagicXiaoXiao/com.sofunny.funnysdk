#if UNITY_ANDROID
using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class AndroidFeedbackService : IBridgeServiceFeedback
    {
        private AndroidOldService Service;

        internal AndroidFeedbackService(AndroidOldService service)
        {
            Service = service;
        }

        public void OpenFeedback(string id = "")
        {
            var parameter = NativeParameter.Builder().Add("playerID", id);

            Service.Call("openFeedback", parameter);
        }
    }
}

#endif