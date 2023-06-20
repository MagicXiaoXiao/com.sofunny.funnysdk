﻿using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoFunny.FunnySDK
{
    internal static class Logger
    {
        const string k_Tag = "[ FunnySDK ]";

        internal const string k_GlobalVerboseLoggingDefine = "ENABLE_FUNNYSDK_DEBUG";

        internal static void Log(object message, ColorStyle style = ColorStyle.Normal)
        {
#if UNITY_EDITOR
            switch (style)
            {
                case ColorStyle.Normal:
                    Debug.unityLogger.Log(k_Tag, $"<color=#F8F8FF>{message}</color>");
                    break;
                case ColorStyle.Green:
                    Debug.unityLogger.Log(k_Tag, $"<color=#ADFF2F>{message}</color>");
                    break;
                case ColorStyle.Red:
                    Debug.unityLogger.Log(k_Tag, $"<color=#FF6347>{message}</color>");
                    break;
                case ColorStyle.Blue:
                    Debug.unityLogger.Log(k_Tag, $"<color=#1E90FF>{message}</color>");
                    break;
                default:
                    Debug.unityLogger.Log(k_Tag, message);
                    break;
            }
#else
            Debug.unityLogger.Log(k_Tag, message);
#endif
        }

        internal static void LogWarning(object message) => Debug.unityLogger.LogWarning(k_Tag, message);

        internal static void LogError(object message)
        {

#if UNITY_EDITOR
            Debug.unityLogger.Log(k_Tag, $"<color=red>{message}</color>");
#else
            Debug.unityLogger.Log(k_Tag, message);
#endif

        }
        internal static void LogException(Exception exception) => Debug.unityLogger.Log(LogType.Exception, k_Tag, exception);

        [Conditional(k_GlobalVerboseLoggingDefine)]
        internal static void LogVerbose(object message) => Debug.unityLogger.Log(k_Tag, message);


        internal enum ColorStyle
        {
            Normal,
            Green,
            Red,
            Blue,
        }

    }
}

