using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace SoFunny.FunnySDKPreview
{

    [Preserve]
    internal static class InteropTasks
    {
        private static readonly Dictionary<string, object> _taskCompletionSources = new Dictionary<string, object>();
        private static readonly object _taskLock = new object();
        private static long _callbackId = long.MinValue;

        internal static TaskCompletionSource<TResult> Create<TResult>(out string callbackId)
        {
            var tcs = new TaskCompletionSource<TResult>();
            lock (_taskLock)
            {
                if (_callbackId == long.MaxValue)
                    _callbackId = long.MinValue;

                _callbackId++;

                callbackId = Convert.ToString(_callbackId);
                _taskCompletionSources.Add(callbackId, tcs);
            }

            return tcs;
        }

        internal static bool TrySetResultAndRemove<TResult>(string callbackId, TResult result)
        {
            TaskCompletionSource<TResult> tcs = null;

            lock (_taskLock)
            {
                if (!_taskCompletionSources.ContainsKey(callbackId))
                    return false;

                tcs = (TaskCompletionSource<TResult>)_taskCompletionSources[callbackId];
                _taskCompletionSources.Remove(callbackId);
            }

            return tcs.TrySetResult(result);
        }

        internal static bool TrySetExceptionAndRemove<TResult>(string callbackId, Exception exception)
        {
            TaskCompletionSource<TResult> tcs = null;

            lock (_taskLock)
            {
                if (!_taskCompletionSources.ContainsKey(callbackId))
                    return false;

                tcs = (TaskCompletionSource<TResult>)_taskCompletionSources[callbackId];
                _taskCompletionSources.Remove(callbackId);
            }

            return tcs.TrySetException(exception);
        }

        internal static bool TryGet<TResult>(string callbackId, out TaskCompletionSource<TResult> tcs)
        {
            tcs = null;

            lock (_taskLock)
            {
                if (!_taskCompletionSources.ContainsKey(callbackId))
                    return false;

                tcs = (TaskCompletionSource<TResult>)_taskCompletionSources[callbackId];
            }

            return true;
        }

        internal static void Remove(string callbackId)
        {
            lock (_taskLock)
            {
                _taskCompletionSources.Remove(callbackId);
            }
        }
    }
}

