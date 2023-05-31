using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class Logger
    {
        const string k_Tag = "[FunnySDK.UI]";

        internal const string k_GlobalVerboseLoggingDefine = "ENABLE_FUNNYSDK_DEBUG";

        public static void Log(object message) => Debug.unityLogger.Log(k_Tag, message);
        public static void LogWarning(object message) => Debug.unityLogger.LogWarning(k_Tag, message);
        public static void LogError(object message) => Debug.unityLogger.LogError(k_Tag, message);
        public static void LogException(Exception exception) => Debug.unityLogger.Log(LogType.Exception, k_Tag, exception);

        [Conditional(k_GlobalVerboseLoggingDefine)]
        public static void LogVerbose(object message) => Debug.unityLogger.Log(k_Tag, message);
    }
}

