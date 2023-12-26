using System;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK Twitter 配置类
    /// </summary>
    public class Twitter
    {
        internal string ConsumerKey;

        internal string ConsumerSecret;

        internal bool Enable
        {
            get
            {
                if (string.IsNullOrEmpty(ConsumerKey) && string.IsNullOrEmpty(ConsumerSecret))
                {
                    return false;
                }

                return true;
            }
        }

        private Twitter() { }

        /// <summary>
        /// 创建 Twitter 配置类
        /// </summary>
        /// <param name="consumerKey">ConsumerKey</param>
        /// <param name="consumerSecret">ConsumerSecret</param>
        /// <returns></returns>
        public static Twitter Create(string consumerKey, string consumerSecret)
        {
            Twitter config = new Twitter();
            config.ConsumerKey = consumerKey;
            config.ConsumerSecret = consumerSecret;

            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static Twitter Empty => Create("", "");

    }

    public interface ITwitterConfiguration
    {
        Twitter SetupTwitter();
    }
}

