﻿using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCFeedbackService : IBridgeServiceFeedback
    {
        internal PCFeedbackService()
        {
        }

        public void OpenFeedback(string id = "")
        {
            Logger.LogWarning("PC 或 Editor 暂未开发此功能");
        }
    }
}

