#if UNITY_IOS

using System;
using System.Runtime.InteropServices;

namespace SoFunny.FunnySDK.Internal
{
    internal class FSDKAnalysisService : IBridgeServiceTrack
    {
        public void TrackEvent(Track track)
        {
            // 后续开发
            //FSDK_TrackEvent(track.JsonData());
        }

        //[DllImport("__Internal")]
        //private static extern void FSDK_TrackEvent(string json);

    }
}

#endif