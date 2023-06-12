using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface ILoginViewEvent
    {
        // 密码登录
        void OnLoginWithPassword(string account, string password);

        // 验证码登录
        void OnLoginWithCode(string account, string code);

        // 第三方登录
        void OnLoginWithProvider();

        // 关闭页面
        void OnCloseView(UILoginPageState pageState);

        /// <summary>
        /// 点击用户协议
        /// </summary>
        void OnClickUserAgreenment();

        /// <summary>
        /// 点击隐私政策
        /// </summary>
        void OnClickPriacyProtocol();

        /// <summary>
        /// 发送验证码
        /// </summary>
        void OnSendVerifcationCode(string account, UILoginPageState pageState);

        /// <summary>
        /// 发起账号注册
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="code"></param>
        void OnRegisterAccount(string account, string pwd, string code);

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="newPwd"></param>
        void OnRetrievePassword(string account, string newPwd, string code);
    }
}

