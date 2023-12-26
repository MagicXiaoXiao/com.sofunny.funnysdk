using System;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK 微信配置类
    /// </summary>
    public class WeChat
    {
        internal string AppID;
        internal string UniversalLinks;

        internal bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(AppID);
            }
        }

        private WeChat() { }

        /// <summary>
        /// 创建微信配置
        /// </summary>
        /// <param name="appID">微信应用 ID</param>
        /// <param name="universalLinks">(iOS 必填)App 通用链接</param>
        /// <returns></returns>
        public static WeChat Create(string appID, string universalLinks = "")
        {
            WeChat config = new WeChat();
            config.AppID = appID;
            config.UniversalLinks = universalLinks;

            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static WeChat Empty => WeChat.Create("");

    }

    internal interface IWeChatConfiguration
    {
        WeChat SetupWeChat();
    }
}

