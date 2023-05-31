using System;
using UnityEngine;


namespace SoFunny.FunnySDK
{

    [Serializable]
    internal class NativeResult {
#pragma warning disable 0649
        [SerializeField] internal bool success;
        [SerializeField] internal string jsonValue;
#pragma warning restore 0649


        internal NativeResult() { }

        internal static NativeResult Create(string json) {
            if (string.IsNullOrEmpty(json)) {
                var result = new NativeResult();
                result.success = false;
                result.jsonValue = "";
                return result;

            }
            return JsonUtility.FromJson<NativeResult>(json);
        }

        internal bool IsSuccess {
            get { return success; }
        }

        internal T TryGetValue<T>() {
            if (string.IsNullOrEmpty(jsonValue)) { return default; }

            if (bool.TryParse(jsonValue, out _)) {
                return (T)Json.Deserialize(jsonValue);
            }
            else {
                return success ? JsonUtility.FromJson<T>(jsonValue) : default;
            }
        }

    }
}

