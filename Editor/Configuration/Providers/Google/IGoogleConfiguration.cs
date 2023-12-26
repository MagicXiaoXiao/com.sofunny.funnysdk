using System;
using System.Linq;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK Google 配置类
    /// </summary>
    public class Google
    {
        internal string ClientID;
        internal string ReversedClientID;

        internal bool EnableGooglePlayGames = false;
        internal string PlayGamesAppID;

        internal bool Enable
        {
            get
            {
                if (string.IsNullOrEmpty(ClientID))
                {
                    return false;
                }

                return true;
            }
        }

        private Google() { }

        /// <summary>
        /// 设置谷歌游戏服务是否开启
        /// </summary>
        /// <param name="enable">是否开启</param>
        /// <param name="appID">游戏服务 ID </param>
        public void SetGooglePlayGames(bool enable, string appID = "")
        {
            EnableGooglePlayGames = enable;
            PlayGamesAppID = appID;
        }

        /// <summary>
        /// 创建 Google 配置类
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reversed"></param>
        /// <returns></returns>
        public static Google Create(string clientID, string reversed = "")
        {
            Google config = new Google();
            config.ClientID = clientID;

            if (string.IsNullOrEmpty(reversed))
            {
                config.ReversedClientID = string.Join(".", clientID.Split('.').Reverse());
            }
            else
            {
                config.ReversedClientID = reversed;
            }

            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static Google Empty => Create("");
    }

    internal interface IGoogleConfiguration
    {
        Google SetupGoogle();
    }
}

