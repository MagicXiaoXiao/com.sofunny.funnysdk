﻿using System;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.Internal
{
    internal delegate void ServiceCompletedHandler<T>(T model, ServiceError error);

    /// <summary>
    /// 各平台服务通用功能实现
    /// </summary>
    internal interface IBridgeServiceBase
    {
        /// <summary>
        /// 服务功能初始化接口
        /// </summary>
        void Initialize();

        /// <summary>
        /// 获取应用配置信息
        /// </summary>
        /// <param name="handler"></param>
        void GetAppInfo(ServiceCompletedHandler<AppInfoConfig> handler);

        /// <summary>
        /// 发送验证码接口
        /// </summary>
        /// <param name="account"></param>
        /// <param name="codeAction"></param>
        /// <param name="codeCategory"></param>
        /// <param name="handler"></param>
        void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler);

        /// <summary>
        /// 打开隐私政策
        /// </summary>
        void OpenPrivacyProtocol();
        /// <summary>
        /// 打开用户协议
        /// </summary>
        void OpenUserAgreenment();

        /// <summary>
        /// 打开用户协议与隐私政策提示界面
        /// </summary>
        void OpenAgreenment();

        /// <summary>
        /// 联系客服
        /// </summary>
        void ContactUS();

        /// <summary>
        /// 唤起原生日期选择器
        /// </summary>
        /// <param name="date"></param>
        /// <param name="handler"></param>
        void ShowDatePicker(string date, ServiceCompletedHandler<string> handler);

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="language"></param>
        void SetLanguage(string language);

    }
}
