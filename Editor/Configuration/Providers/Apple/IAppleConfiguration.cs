using System;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// FunnySDK Apple 配置类
    /// </summary>
    public class Apple
    {
        internal bool EnableSignIn = false;

        private Apple() { }

        /// <summary>
        /// 创建 Apple 配置类
        /// </summary>
        /// <param name="enableSignIn"></param>
        /// <returns></returns>
        public static Apple Create(bool enableSignIn)
        {
            Apple config = new Apple();
            config.EnableSignIn = enableSignIn;

            return config;
        }

        /// <summary>
        /// 空配置，表示不开启相关服务
        /// </summary>
        public static Apple Empty => Create(false);
    }

    /// <summary>
    /// 苹果服务相关配置项
    /// </summary>
    internal interface IAppleConfiguration
    {
        Apple SetupApple();
    }
}

