using System;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK Facebook 配置类
    /// </summary>
    public class Facebook
    {
        internal string AppID;

        internal string ClientToken;

        internal bool EnableAdvertiserTrack = false;

        internal bool Enable
        {
            get
            {
                if (string.IsNullOrEmpty(AppID) && string.IsNullOrEmpty(ClientToken))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 是否开启 Facebook Adv 服务
        /// </summary>
        /// <param name="enable"></param>
        public void SetAdvertiserTracking(bool enable)
        {
            EnableAdvertiserTrack = enable;
        }

        private Facebook() { }

        /// <summary>
        /// 创建 Facebook 配置类
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        public static Facebook Create(string appID, string clientToken)
        {
            Facebook config = new Facebook();
            config.AppID = appID;
            config.ClientToken = clientToken;
            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static Facebook Empty => Create("", "");

    }

    internal interface IFacebookConfiguration
    {
        Facebook SetupFacebook();
    }
}

