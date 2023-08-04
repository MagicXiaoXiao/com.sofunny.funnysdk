using System;

namespace SoFunny.FunnySDK.Internal
{
    internal class PrivateInfoAuthTrack
    {
        private readonly IBridgeServiceTrack TrackService;

        internal PrivateInfoAuthTrack(IBridgeServiceTrack service)
        {
            TrackService = service;
        }

        /// <summary>
        /// 开始隐私信息授权
        /// </summary>
        internal void Start()
        {
            Track track = Track.Event("sdk_get_account_info")
                               .Add("ui_type", 1)
                               .Add("status", -1);
            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 取消了隐私授权
        /// </summary>
        internal void Cancel()
        {
            Track track = Track.Event("sdk_get_account_info")
                               .Add("ui_type", 1)
                               .Add("status", 2);
            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 服务未开启
        /// </summary>
        internal void NotEnabled()
        {
            Track track = Track.Event("sdk_get_account_info")
                               .Add("ui_type", 1)
                               .Add("status", 3);
            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 隐私授权成功
        /// </summary>
        internal void SuccessResult()
        {
            Track track = Track.Event("sdk_get_account_info")
                               .Add("ui_type", 1)
                               .Add("status", 1);
            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 隐私授权失败
        /// </summary>
        /// <param name="error"></param>
        internal void FailureResult(ServiceError error)
        {
            Track track = Track.Event("sdk_get_account_info")
                               .Add("ui_type", 1)
                               .Add("status", 0)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);
            TrackService.TrackEvent(track);
        }

    }
}

