#if UNITY_EDITOR || UNITY_STANDALONE

using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class PCAnalysisService : IBridgeServiceTrack
    {
        internal PCAnalysisService()
        {

        }

        void IBridgeServiceTrack.TrackEvent(Track track)
        {
            Logger.Log($"PC 埋点 - {track.Name} - {track.JsonData()}", Logger.ColorStyle.Blue);
        }
    }
}

#endif
