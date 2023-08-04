using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface ILoginViewEvent
    {
        /// <summary>
        /// 切换其他账号行为
        /// </summary>
        void OnSwitchOtherAccount();

        /// <summary>
        /// 打开登录相关界面行为
        /// </summary>
        /// <param name="current"></param>
        /// <param name="prev"></param>
        void OnOpenView(UILoginPageState current, UILoginPageState prev);

        /// <summary>
        /// 关闭登录相关界面
        /// </summary>
        /// <param name="pageState"></param>
        void OnCloseView(UILoginPageState pageState);

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        void OnLoginWithPassword(string account, string password);

        /// <summary>
        /// 验证码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        void OnLoginWithCode(string account, string code);

        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <param name="provider"></param>
        void OnLoginWithProvider(LoginProvider provider);

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

        /// <summary>
        /// 邀请码提交
        /// </summary>
        /// <param name="code"></param>
        void OnActivationCodeCommit(string code);

        /// <summary>
        /// 实名认证信息提交
        /// </summary>
        /// <param name="realname"></param>
        /// <param name="cardID"></param>
        void OnRealnameInfoCommit(string realname, string cardID);

        /// <summary>
        /// 点击联系客服
        /// </summary>
        void OnClickContactUS();

        /// <summary>
        /// 撤销删除
        /// </summary>
        void OnReCallDelete();
    }
}

