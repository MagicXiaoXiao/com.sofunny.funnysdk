using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace SoFunny.FunnySDK.Editor {

    public partial class FunnyConfig {

        private readonly static Lazy<FunnyConfig> lazy = new Lazy<FunnyConfig>(() => new FunnyConfig());

        private FunnyConfig() {
            SyncData();
        }

        internal static FunnyConfig Instance {
            get {
                return lazy.Value;
            }
        }

        private const string fileName = "Configuration.json";
        private const string configFilePath = "Packages/FunnyConfig";
        private string configFullPath = Path.Combine(configFilePath, fileName);

        internal readonly string _defaultPluginPath = "Packages/com.sofunny.funnysdk";

    }

    [Serializable]
    public partial class FunnyConfig {
        
        [SerializeField]
        public bool isMainland;

        [SerializeField]
        public string appID;

        [SerializeField]
        public GoogleConfig Google = new GoogleConfig();

        [SerializeField]
        public FacebookConfig Facebook = new FacebookConfig();

        [SerializeField]
        public TwitterConfig Twitter = new TwitterConfig();

        [SerializeField]
        public AppleSignIn Apple = new AppleSignIn();

        [SerializeField]
        public WeChatConfig WeChat = new WeChatConfig();

        [SerializeField]
        public TencentQQConfig QQ = new TencentQQConfig();

        [SerializeField]
        public TapTapConfig TapTap = new TapTapConfig();

        // 同步配置文件数据
        public void SyncData() {
            CheckConfigFile();

            var jsonText = File.ReadAllText(configFullPath);
            EditorJsonUtility.FromJsonOverwrite(jsonText, this);
        }
        // 检查是否存在配置文件，不存在则创建
        private void CheckConfigFile() {
            if (!File.Exists(configFullPath)) {
                Directory.CreateDirectory(configFilePath);
                using (File.Create(configFullPath)) { }
            }
        }
        /// <summary>
        /// 将数据同步到配置文件中
        /// </summary>
        public void SyncToConfig() {
            CheckConfigFile();
            var jsonText = EditorJsonUtility.ToJson(this);
            File.WriteAllText(configFullPath, jsonText, System.Text.Encoding.UTF8);
        }
    }

    public partial class FunnyConfig {


    }

    #region 平台配置结构

    [Serializable]
    public struct GoogleConfig {
        [SerializeField]
        public string idToken;

        public GoogleConfig(string idToken) {
            this.idToken = idToken;
        }
        /// <summary>
        /// 是否设置了 Google 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(idToken);
            }
        }
    }

    [Serializable]
    public struct FacebookConfig {
        [SerializeField]
        public string appID;

        [SerializeField]
        public string clientToken;

        [SerializeField]
        public bool trackEnable;

        public FacebookConfig(string appID, string clientToken, bool trackEnable = false) {
            this.appID = appID;
            this.clientToken = clientToken;
            this.trackEnable = trackEnable;
        }

        /// <summary>
        /// 是否设置了 Facebook 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(appID) && !string.IsNullOrEmpty(clientToken);
            }
        }
    }

    [Serializable]
    public struct TwitterConfig {
        [SerializeField]
        public string consumerKey;

        [SerializeField]
        public string consumerSecret;

        public TwitterConfig(string consumerKey, string consumerSecret) {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        /// <summary>
        /// 是否设置了 Twitter 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(consumerKey) && !string.IsNullOrEmpty(consumerSecret);
            }
        }

    }

    [Serializable]
    public struct AppleSignIn {
        [SerializeField]
        public bool overseaEnable;
        [SerializeField]
        public bool mainlandEnable;
    }

    [Serializable]
    public struct WeChatConfig {
        [SerializeField]
        public string appID;
        [SerializeField]
        public string universalLink;

        public WeChatConfig(string appID, string universalLink) {
            this.appID = appID;
            this.universalLink = universalLink;
        }

        /// <summary>
        /// 是否设置了 WeChat 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(appID) && !string.IsNullOrEmpty(universalLink);
            }
        }

    }

    [Serializable]
    public struct TencentQQConfig {
        [SerializeField]
        public string appID;
        [SerializeField]
        public string universalLink;

        internal TencentQQConfig(string appID, string universalLink) {
            this.appID = appID;
            this.universalLink = universalLink;
        }

        /// <summary>
        /// 是否设置了 QQ 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(appID);
            }
        }

    }


    [Serializable]
    public struct TapTapConfig {
        [SerializeField]
        public string clientID;
        [SerializeField]
        public string clientToken;
        [SerializeField]
        public string serverURL;
        [SerializeField]
        public bool isBonfire;

        public TapTapConfig(string clientID, string clientToken, string serverURL, bool isBonfire)
        {
            this.clientID = clientID;
            this.clientToken = clientToken;
            this.serverURL = serverURL;
            this.isBonfire = isBonfire;
        }

        /// <summary>
        /// 是否设置了 TapTap 相关参数
        /// </summary>
        public bool Enable {
            get {
                return !string.IsNullOrEmpty(clientID) && !string.IsNullOrEmpty(clientToken) && !string.IsNullOrEmpty(serverURL);
            }
        }
    }

    #endregion
}

