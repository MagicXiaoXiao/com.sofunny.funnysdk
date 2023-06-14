using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoFunny.FunnySDK
{
    internal static class Logger
    {
        const string k_Tag = "[ FunnySDK ]";

        internal const string k_GlobalVerboseLoggingDefine = "ENABLE_FUNNYSDK_DEBUG";

        internal static void Log(object message) => Debug.unityLogger.Log(k_Tag, message);
        internal static void LogWarning(object message) => Debug.unityLogger.LogWarning(k_Tag, message);
        internal static void LogError(object message) => Debug.unityLogger.Log(k_Tag, $"<color=red>{message}</color>");
        internal static void LogException(Exception exception) => Debug.unityLogger.Log(LogType.Exception, k_Tag, exception);

        [Conditional(k_GlobalVerboseLoggingDefine)]
        internal static void LogVerbose(object message) => Debug.unityLogger.Log(k_Tag, message);
    }
}

