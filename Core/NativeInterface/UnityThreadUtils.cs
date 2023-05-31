﻿using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    internal static class UnityThreadUtils
    {
        static int s_UnityThreadId;

        internal static TaskScheduler UnityThreadScheduler;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void CaptureUnityThreadInfo() {
            s_UnityThreadId = Thread.CurrentThread.ManagedThreadId;
            UnityThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public static bool IsRunningOnUnityThread => Thread.CurrentThread.ManagedThreadId == s_UnityThreadId;
    }
}

