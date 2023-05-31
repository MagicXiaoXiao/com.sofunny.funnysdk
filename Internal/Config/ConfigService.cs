using System;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    public static class ConfigService
    {
        private static Lazy<SDKConfig> _config = new Lazy<SDKConfig>(() =>
        {
            return Resources.Load<SDKConfig>("FunnySDK/SDKConfig/SDKConfig");
        }, true);

        public static SDKConfig Config
        {
            get
            {
                return _config.Value;
            }
        }
    }
}

