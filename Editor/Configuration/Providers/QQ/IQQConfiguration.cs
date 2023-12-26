using System;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK 腾讯QQ 配置类
    /// </summary>
    public class QQ
    {
        internal string AppID;
        internal string UniversalLinks;

        internal bool Enable
        {
            get
            {
                if (string.IsNullOrEmpty(AppID))
                {
                    return false;
                }

                return true;
            }
        }

        private QQ() { }

        /// <summary>
        /// 创建 QQ 配置类
        /// </summary>
        /// <param name="appID">QQ 应用 ID</param>
        /// <param name="universalLinks">App 通用连接</param>
        /// <returns></returns>
        public static QQ Create(string appID, string universalLinks = "")
        {
            QQ config = new QQ();
            config.AppID = appID;
            config.UniversalLinks = universalLinks;

            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static QQ Empty => QQ.Create("", "");

    }

    internal interface IQQConfiguration
    {
        QQ SetupQQ();
    }

}

