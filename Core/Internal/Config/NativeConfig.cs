using System;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{

    [JsonObject(MemberSerialization.OptIn)]
    public class NativeConfig
    {
        [JsonProperty("appID")]
        public string AppID { get; set; }

        [JsonProperty("env")]
        public PackageEnv Env { get; set; }

        [JsonProperty("webURL")]
        public string WebURL { get; set; }

    }
}

