using System;
using Newtonsoft.Json;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// SDK 环境配置
    /// </summary>
    public enum FunnyEnv
    {
        /// <summary>
        /// 国内环境
        /// </summary>
        Mainland,
        /// <summary>
        /// 海外环境
        /// </summary>
        Overseas,
        /// <summary>
        /// 内部测试
        /// </summary>
        InternalBeta,
        /// <summary>
        /// 邀请测试
        /// </summary>
        InvitationBeta,
    }

    /// <summary>
    /// 初始化参数配置
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InitConfig
    {
        [JsonProperty("appid")]
        public string AppID { get; internal set; }

        [JsonProperty("env")]
        public FunnyEnv Env { get; internal set; }

        private InitConfig() { }

        /// <summary>
        /// 创建初始化配置
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static InitConfig Create(string appID, FunnyEnv env)
        {
            InitConfig config = new InitConfig();
            config.AppID = appID;
            config.Env = env;

            return config;
        }

        internal NativeConfig GenerateNativeConfig()
        {
            NativeConfig config = new NativeConfig();
            config.AppID = AppID;

            switch (Env)
            {
                case FunnyEnv.Mainland:
                    config.Env = PackageEnv.Mainland;
                    break;
                case FunnyEnv.Overseas:
                    config.Env = PackageEnv.Overseas;
                    break;
                case FunnyEnv.InternalBeta:
                    config.Env = PackageEnv.InternalBeta;
                    break;
                case FunnyEnv.InvitationBeta:
                    config.Env = PackageEnv.InvitationBeta;
                    break;
            }

            return config;
        }

    }
}

