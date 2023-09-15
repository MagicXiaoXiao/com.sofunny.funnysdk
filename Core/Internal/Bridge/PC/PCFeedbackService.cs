#if UNITY_EDITOR || UNITY_STANDALONE

using System;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCFeedbackService : IBridgeServiceFeedback
    {
        internal PCFeedbackService()
        {
        }

        public void OpenFeedback(string id = "")
        {
            Toast.ShowFail("此功能暂未开放");
            Logger.LogWarning("PC 或 Editor 暂未开发此功能");
        }
    }
}

#endif

