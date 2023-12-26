
#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    public class QQAndroidBuildStep : AndroidBaseBuildStep
    {
        private QQ qqConfig;
        private bool _enable = false;

        public override bool IsEnabled => _enable;

        internal override void OnInitConfig(Configuration.Android config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (!editorConfig.IsMainland) return;

                qqConfig = QQ.Create(editorConfig.QQ.appID, editorConfig.QQ.universalLink);
                _enable = qqConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                qqConfig = config.SetupQQ();

                if (initConfig.Env == FunnyEnv.Overseas)
                {
                    _enable = qqConfig.Enable;
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
                                    return aar.Name.Equals("funny-sdk-qq.aar");
                                });

            return allAARFiles.ToArray();
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");

            depNode.AppendContentNode("implementation 'com.tencent.tauth:qqopensdk:3.52.0'");
            depNode.AppendContentNode("implementation(name: 'funny-sdk-qq', ext:'aar')");
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement qqAppID = stringsXML.CreateElement("string");
            qqAppID.SetAttribute("name", "qq_app_id");
            qqAppID.InnerText = qqConfig.AppID;

            resources.AppendChild(qqAppID);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement qqNode = manifestXML.CreateElement("meta-data");
            qqNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.QQAppId");
            qqNode.SetAttribute("value", NamespaceURI, "@string/qq_app_id");
            applicationNode.AppendChild(qqNode);

            XmlElement orgLegacyNode = manifestXML.CreateElement("uses-library");
            orgLegacyNode.SetAttribute("name", NamespaceURI, "org.apache.http.legacy");
            orgLegacyNode.SetAttribute("required", NamespaceURI, "false");
            applicationNode.AppendChild(orgLegacyNode);
        }
    }
}

#endif