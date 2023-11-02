using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{

    [JsonObject(MemberSerialization.OptIn)]
    internal class NativeConfig
    {
        [JsonProperty("appID")]
        internal string AppID;

        [JsonProperty("mainland")]
        internal bool IsMainland;

        [JsonProperty("webURL")]
        internal string webURL;

    }
}

