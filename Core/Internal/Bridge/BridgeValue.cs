using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal class BridgeValue
    {
        public static BridgeValue Empty = new BridgeValue();

        public static BridgeValue Create(string value)
        {
            return new BridgeValue(value);
        }

        private readonly string optional;

        private BridgeValue()
        {
            optional = "";
        }

        private BridgeValue(string value)
        {
            optional = value;
        }

        internal bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(optional);
            }
        }

        internal string RawValue => optional;

        internal bool TryGet<T>(out T target)
        {
            target = default;

            if (string.IsNullOrEmpty(optional))
            {
                return false;
            }
            try
            {
                target = JsonConvert.DeserializeObject<T>(optional);
                return true;
            }
            catch (JsonException ex)
            {
                Logger.LogError("BridgeValue Deserialize error. " + ex.Message);
                return false;
            }

        }
    }
}

