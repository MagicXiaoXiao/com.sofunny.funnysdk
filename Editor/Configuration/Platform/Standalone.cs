using System;

namespace SoFunny.FunnySDK.Editor
{
    public partial class Configuration
    {
        public abstract partial class Standalone : ISoFunnyConfiguration
        {
            /// <summary>
            /// 设置初始化参数配置
            /// </summary>
            /// <returns></returns>
            public abstract InitConfig SetupInit();
        }



    }

}

