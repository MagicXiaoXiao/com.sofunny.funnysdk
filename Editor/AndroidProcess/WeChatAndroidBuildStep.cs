#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    public class WeChatAndroidBuildStep : AndroidBaseBuildStep
    {
        private WeChat wechatConfig;
        private bool _enable = false;

        public override bool IsEnabled => _enable;

        internal override void OnInitConfig(Configuration.Android config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (!editorConfig.IsMainland) return;

                wechatConfig = WeChat.Create(editorConfig.WeChat.appID, editorConfig.WeChat.universalLink);
                _enable = wechatConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                wechatConfig = config.SetupWeChat();

                if (initConfig.Env == FunnyEnv.Mainland)
                {
                    _enable = wechatConfig.Enable;
                }

            }
        }

        public override FileInfo[] OnProcessPrepareAARFile(string unityLibraryPath)
        {
            var allAARFiles = Directory.GetFiles(AAR_ORIGIN_PATH)
                                .Where((dirPath) =>
                                {
                                    return Path.GetExtension(dirPath) == ".aar";
                                })
                                .Select((dirPath) =>
                                {
                                    return new FileInfo(dirPath);
                                })
                                .Where((aar) =>
                                {
                                    if (aar.Name.Equals("funny-sdk-wechat.aar"))
                                    {
                                        return true;
                                    }
                                    return false;
                                });

            return allAARFiles.ToArray();
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");
            depNode.AppendContentNode("implementation 'com.tencent.mm.opensdk:wechat-sdk-android:6.8.0'");
            depNode.AppendContentNode("implementation(name: 'funny-sdk-wechat', ext:'aar')");
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement wechatAppID = stringsXML.CreateElement("string");
            wechatAppID.SetAttribute("name", "wechat_app_id");
            wechatAppID.InnerText = wechatConfig.AppID;
            resources.AppendChild(wechatAppID);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement wechatNode = manifestXML.CreateElement("meta-data");
            wechatNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.WeChatAppId");
            wechatNode.SetAttribute("value", NamespaceURI, "@string/wechat_app_id");
            applicationNode.AppendChild(wechatNode);
        }

        public override void OnProcessLauncherGradle(GradleConfig gradle)
        {
            var defaultConfig = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("defaultConfig");
            defaultConfig.AppendContentNode("manifestPlaceholders = [\nwechat_package: applicationId\n]");
        }

    }
}

#endif