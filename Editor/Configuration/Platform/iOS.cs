using System;

namespace SoFunny.FunnySDK.Editor
{
    public partial class Configuration
    {
        #region 基础配置
#pragma warning disable IDE1006 // 忽略命名样式
        public abstract partial class iOS : ISoFunnyConfiguration
#pragma warning restore IDE1006 // 忽略命名样式
        {

            /// <summary>
            /// 设置初始化参数配置
            /// </summary>
            /// <returns></returns>
            public abstract InitConfig SetupInit();

        }

        #endregion

        #region Apple 配置

        public partial class iOS : IAppleConfiguration
        {
            /// <summary>
            /// 设置 Apple 服务相关参数配置
            /// </summary>
            /// <returns></returns>
            public virtual Apple SetupApple()
            {
                return Apple.Empty;
            }
        }

        #endregion

        #region WeChat 配置

        public partial class iOS : IWeChatConfiguration
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

        public partial class iOS : IQQConfiguration
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

        public partial class iOS : ITapTapConfiguration
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

        public partial class iOS : IGoogleConfiguration
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

        public partial class iOS : IFacebookConfiguration
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

        public partial class iOS : ITwitterConfiguration
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

