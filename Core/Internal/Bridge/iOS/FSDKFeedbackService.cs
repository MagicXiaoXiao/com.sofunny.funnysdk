#if UNITY_IOS

using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKFeedbackService : IBridgeServiceFeedback
    {
        internal FSDKFeedbackService()
        {
        }

        public void OpenFeedback(string id = "")
        {
            var service = FSDKCall.Builder("OpenFeedback");

            if (!string.IsNullOrEmpty(id))
            {
                service.Add("id", id);
            }

            service.Invoke();
        }
    }
}

#endif
