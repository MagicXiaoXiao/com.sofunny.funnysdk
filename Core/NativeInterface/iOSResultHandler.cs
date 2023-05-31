using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
#if UNITY_IOS
    internal class iOSResultHandler<T> : INativeResultDecodeHandler {
        internal iOSResultHandler() {
        }

        public void DecodeResult(string taskID, NativeResult result) {
            if (result.success) {
                var value = result.TryGetValue<T>();
                InteropTasks.TrySetResultAndRemove(taskID, value);

            }
            else {
                var exception = JsonUtility.FromJson<FunnyError>(result.jsonValue);
                InteropTasks.TrySetExceptionAndRemove<T>(taskID, exception.MatchException());
            }
        }
    }
#endif
}

