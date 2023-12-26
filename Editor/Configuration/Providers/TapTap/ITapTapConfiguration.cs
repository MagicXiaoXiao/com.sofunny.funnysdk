using System;

namespace SoFunny.FunnySDK.Editor
{
    public class TapTap
    {
        internal string ClientID;
        internal string ClientToken;
        internal string ServerURL;
        internal bool EnableTestVersion = false;

        internal bool Enable
        {
            get
            {
                if (string.IsNullOrEmpty(ClientID) || string.IsNullOrEmpty(ClientToken) || string.IsNullOrEmpty(ServerURL))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 开启小号测试
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableTest(bool enable)
        {
            EnableTestVersion = enable;
        }

        private TapTap() { }

        public static TapTap Create(string clientID, string clientToken, string serverURL)
        {
            TapTap config = new TapTap();
            config.ClientID = clientID;
            config.ClientToken = clientToken;
            config.ServerURL = serverURL;

            return config;
        }

        public static TapTap Empty => Create("", "", "");

    }

    internal interface ITapTapConfiguration
    {
        TapTap SetupTapTap();
    }

}

