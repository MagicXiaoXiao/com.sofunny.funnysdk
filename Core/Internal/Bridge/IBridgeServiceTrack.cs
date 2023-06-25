using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 埋点功能接口
    /// </summary>
    internal interface IBridgeServiceTrack
    {
        /// <summary>
        /// 上报埋点
        /// </summary>
        /// <param name="track"></param>
        void TrackEvent(Track track);
    }
}

