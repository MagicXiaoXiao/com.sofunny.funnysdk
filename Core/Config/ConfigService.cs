using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class ConfigService
    {
        private static Lazy<FunnySDKConfig> _config = new Lazy<FunnySDKConfig>(() =>
        {
            return Resources.Load<FunnySDKConfig>("FunnySDK/ServiceConfig");
        }, true);

        internal static FunnySDKConfig Config
        {
            get
            {
                return _config.Value;
            }
        }
    }
}

