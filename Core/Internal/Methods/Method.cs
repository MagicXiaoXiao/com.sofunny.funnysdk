using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 对接方法结构体
    /// </summary>
    public struct Method
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        public const string Initialize = "Initialize";
        /// <summary>
        /// 密码登录
        /// </summary>
        public const string LoginWithPWD = "LoginWithPWD";
        /// <summary>
        /// 账号注册
        /// </summary>
        public const string RegisterAccount = "RegisterAccount";

        /// <summary>
        /// 发送验证码
        /// </summary>
        public const string SendVerificationCode = "SendVerificationCode";

    }

}

