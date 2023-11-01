using System;
using System.Collections.Generic;
using SoFunny.FunnySDK.Promises;

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
        Promise<AppInfoConfig> GetAppInfo();

        /// <summary>
        /// 发送验证码接口
        /// </summary>
        /// <param name="account"></param>
        /// <param name="codeAction"></param>
        /// <param name="codeCategory"></param>
        Promise SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory);

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
        Promise<string> ShowDatePicker(string date);

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="language"></param>
        void SetLanguage(string language);

    }
}

