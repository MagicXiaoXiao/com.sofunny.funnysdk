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
#if UNITY_EDITOR
            Logger.Log($"PC 埋点 - {track.Name} - {track.JsonData()}", Logger.ColorStyle.Blue);
#endif
        }
    }
}

