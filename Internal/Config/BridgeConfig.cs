using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class BridgeConfig
    {
        internal static string AppID { get; private set; }
        internal static bool IsMainland { get; private set; }

        internal static void Init(string appID, bool isMainland)
        {
            AppID = appID;
            IsMainland = isMainland;
        }

    }
}

