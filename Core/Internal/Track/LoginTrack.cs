using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoFunny.FunnySDK.Internal
{
    internal class LoginTrack
    {
        private readonly IBridgeServiceTrack TrackService;

        private Guid LoginID;
        private int IsAuto = 0;
        private int LoginWay = -1;
        private int LoginFrom = -1;
        private Stopwatch Watch;


        internal LoginTrack(IBridgeServiceTrack service)
        {
            TrackService = service;
            LoginID = Guid.Empty;
            Watch = new Stopwatch();
        }

        /// <summary>
        /// 发起登录流程
        /// </summary>
        private void StartFlow()
        {
            if (LoginID == Guid.Empty)
            {
                LoginID = Guid.NewGuid();
            }

            Watch.Start();
        }

        /// <summary>
        /// 结束登录流程
        /// </summary>
        private void EndFlow()
        {
            IsAuto = 0;
            LoginWay = -1;
            LoginFrom = -1;
            LoginID = Guid.Empty;
        }

        private Dictionary<string, object> GlobalData()
        {
            return new Dictionary<string, object>()
            {
                {"sdk_login_id",LoginID.ToString("N") },
                {"ui_type",1 }
            };
        }

        internal void SetLoginWay(int loginWay)
        {
            LoginWay = loginWay;
        }

        internal void SetLoginFrom(int loginFrom)
        {
            LoginFrom = loginFrom;
        }

        /// <summary>
        /// 唤起登录相关页面
        /// </summary>
        /// <param name="pageID"></param>
        internal void SdkPageOpen(int pageID)
        {
            StartFlow();

            Track track = Track.Event("sdk_page_open")
                               .AddData(GlobalData())
                               .Add("page", pageID);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 关闭登录相关页面
        /// </summary>
        /// <param name="pageID"></param>
        internal void SdkPageClose(int pageID)
        {
            Track track = Track.Event("sdk_close_window")
                               .AddData(GlobalData())
                               .Add("page", pageID);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 登录页面切换
        /// </summary>
        /// <param name="currentPageID"></param>
        /// <param name="prevPageID"></param>
        internal void SdkPageLoad(int currentPageID, int prevPageID)
        {
            Track track = Track.Event("sdk_page_load")
                               .AddData(GlobalData())
                               .Add("from_page", prevPageID)
                               .Add("page", currentPageID)
                               .Add("status", 1);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 发起登录行为成功
        /// </summary>
        /// <param name="loginWay"></param>
        /// <param name="isAuto"></param>
        /// <param name="agreedPolicy"></param>
        internal void SdkStartLoginSuccess(bool isAuto, bool agreedPolicy)
        {
            IsAuto = isAuto ? 1 : 0;

            Track track = Track.Event("sdk_start_login")
                               .AddData(GlobalData())
                               .Add("is_auto", IsAuto)
                               .Add("agreed_policy", agreedPolicy ? 1 : 0)
                               .Add("status", 1);

            if (LoginWay > 0)
            {
                track.Add("login_way", LoginWay);
            }

            TrackService.TrackEvent(track);

        }

        /// <summary>
        /// 发起登录行为失败
        /// </summary>
        /// <param name="loginWay"></param>
        /// <param name="isAuto"></param>
        /// <param name="agreedPolicy"></param>
        /// <param name="error"></param>
        internal void SdkStartLoginFailure(bool isAuto, bool agreedPolicy, ServiceError error)
        {
            IsAuto = isAuto ? 1 : 0;

            Track track = Track.Event("sdk_start_login")
                               .AddData(GlobalData())
                               .Add("is_auto", IsAuto)
                               .Add("agreed_policy", agreedPolicy ? 1 : 0)
                               .Add("status", 0)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);

            if (LoginWay > 0)
            {
                track.Add("login_way", LoginWay);
            }

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 第三方授权取消
        /// </summary>
        /// <param name="loginWay"></param>
        internal void SdkTPAuthCancel()
        {
            Track track = Track.Event("sdk_third_party_authorization")
                               .AddData(GlobalData())
                               .Add("login_way", LoginWay)
                               .Add("status", 2)
                               .Add("error_code", -1)
                               .Add("error_msg", "第三方授权被取消");

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 第三方授权失败
        /// </summary>
        /// <param name="loginWay"></param>
        /// <param name="error"></param>
        internal void SdkTPAuthFailure(ServiceError error)
        {
            Track track = Track.Event("sdk_third_party_authorization")
                               .AddData(GlobalData())
                               .Add("login_way", LoginWay)
                               .Add("status", 0)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 发送验证码成功
        /// </summary>
        /// <param name="pageID"></param>
        internal void SdkSendCodeSuccess(int pageID)
        {
            Track track = Track.Event("sdk_send_code")
                               .AddData(GlobalData())
                               .Add("page", pageID)
                               .Add("status", 1);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 发送验证码失败
        /// </summary>
        /// <param name="pageID"></param>
        /// <param name="error"></param>
        internal void SdkSendCodeFailure(int pageID, ServiceError error)
        {
            Track track = Track.Event("sdk_send_code")
                               .AddData(GlobalData())
                               .Add("page", pageID)
                               .Add("status", 0)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 验证码校验成功
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="verifyReason"></param>
        internal void SdkVerifyCodeSuccess(int codeType, int verifyReason)
        {
            Track track = Track.Event("sdk_verify_code")
                               .AddData(GlobalData())
                               .Add("code_type", codeType)
                               .Add("verify_reason", verifyReason)
                               .Add("status", 1);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 验证码校验失败
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="verifyReason"></param>
        /// <param name="error"></param>
        internal void SdkVerifyCodeFailure(int codeType, int verifyReason, ServiceError error)
        {
            Track track = Track.Event("sdk_verify_code")
                               .AddData(GlobalData())
                               .Add("code_type", codeType)
                               .Add("verify_reason", verifyReason)
                               .Add("status", 0)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);

            TrackService.TrackEvent(track);
        }

        /// <summary>
        /// 登录结果成功
        /// </summary>
        /// <param name="isRegister"></param>
        internal void SdkLoginResultSuccess(bool isRegister)
        {
            Watch.Stop();

            Track track = Track.Event("sdk_login_result")
                               .AddData(GlobalData())
                               .Add("is_auto", IsAuto)
                               .Add("is_register", isRegister ? 1 : 0)
                               .Add("status", 1)
                               .Add("duration", Watch.Elapsed.Milliseconds);

            if (LoginFrom > 0)
            {
                track.Add("login_from", LoginFrom);
            }

            TrackService.TrackEvent(track);

            EndFlow();
        }

        /// <summary>
        /// 登录结果失败
        /// </summary>
        /// <param name="isRegister"></param>
        /// <param name="error"></param>
        internal void SdkLoginResultFailure(bool isRegister, ServiceError error)
        {
            Watch.Stop();

            Track track = Track.Event("sdk_login_result")
                               .AddData(GlobalData())
                               .Add("is_auto", IsAuto)
                               .Add("is_register", isRegister ? 1 : 0)
                               .Add("status", 0)
                               .Add("duration", Watch.Elapsed.Milliseconds)
                               .Add("error_code", error.Code)
                               .Add("error_msg", error.Message);

            if (LoginFrom > 0)
            {
                track.Add("login_from", LoginFrom);
            }

            TrackService.TrackEvent(track);

            EndFlow();
        }

    }
}

