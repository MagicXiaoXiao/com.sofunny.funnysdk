
#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    public class QQAndroidBuildStep : AndroidBaseBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled
        {
            get
            {
                return Config.IsMainland && Config.QQ.Enable;
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
            qqAppID.InnerText = Config.QQ.appID;

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

        public override void OnProcessLauncherGradle(GradleConfig gradle) {
            var defaultConfig = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("defaultConfig");
            defaultConfig.AppendContentNode($"manifestPlaceholders[\"qq_auth_app_id\"] = \"{Config.QQ.appID}\"");
        }

    }
}

#endif