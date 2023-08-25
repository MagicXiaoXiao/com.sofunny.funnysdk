#if UNITY_ANDROID

using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class NativeResult
    {
        [JsonProperty("success")]
        internal bool success;

        [JsonProperty("jsonValue")]
        internal string jsonValue;

        internal NativeResult() { }

        internal static NativeResult Create(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var result = new NativeResult();
                result.success = false;
                result.jsonValue = "";
                return result;

            }
            try
            {
                var result = JsonConvert.DeserializeObject<NativeResult>(json);

                return result;
            }
            catch (JsonException ex)
            {
                Logger.Log("NativeResult Error - " + ex.Message);
                return default;
            }
        }

        internal bool IsSuccess
        {
            get { return success; }
        }

        internal T TryGetValue<T>()
        {
            if (string.IsNullOrEmpty(jsonValue)) { return default; }

            if (bool.TryParse(jsonValue, out _))
            {
                return JsonConvert.DeserializeObject<T>(jsonValue);
            }
            else
            {
                return success ? JsonConvert.DeserializeObject<T>(jsonValue) : default;
            }
        }
    }
}

#endif

