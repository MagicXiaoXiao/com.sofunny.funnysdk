using System;
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
        /// 发送验证码接口
        /// </summary>
        /// <param name="account"></param>
        /// <param name="codeAction"></param>
        /// <param name="codeCategory"></param>
        /// <param name="handler"></param>
        void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler);

    }
}

