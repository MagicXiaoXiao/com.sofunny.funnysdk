using System;

namespace SoFunny.FunnySDK.Editor
{
    public partial class Configuration
    {
        #region 基础配置

        public abstract partial class Android : ISoFunnyConfiguration
        {
            /// <summary>
            /// 设置初始化参数配置
            /// </summary>
            /// <returns></returns>
            public abstract InitConfig SetupInit();
        }

        #endregion

        #region WeChat 配置

        public partial class Android : IWeChatConfiguration
        {
            /// <summary>
            /// 设置 WeChat 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual WeChat SetupWeChat()
            {
                return WeChat.Empty;
            }
        }

        #endregion

        #region QQ 配置

        public partial class Android : IQQConfiguration
        {
            /// <summary>
            /// 设置 QQ 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual QQ SetupQQ()
            {
                return QQ.Empty;
            }
        }

        #endregion

        #region TapTap 配置

        public partial class Android : ITapTapConfiguration
        {
            /// <summary>
            /// 设置 TapTap 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual TapTap SetupTapTap()
            {
                return TapTap.Empty;
            }
        }

        #endregion

        #region Google 配置

        public partial class Android : IGoogleConfiguration
        {
            /// <summary>
            /// 设置 Google 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual Google SetupGoogle()
            {
                return Google.Empty;
            }
        }

        #endregion

        #region Facebook 配置

        public partial class Android : IFacebookConfiguration
        {
            /// <summary>
            /// 设置 Facebook 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual Facebook SetupFacebook()
            {
                return Facebook.Empty;
            }
        }

        #endregion

        #region Twitter 配置

        public partial class Android : ITwitterConfiguration
        {
            /// <summary>
            /// 设置 Twitter 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual Twitter SetupTwitter()
            {
                return Twitter.Empty;
            }
        }

        #endregion
    }

}

