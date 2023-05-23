
#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    /// <summary>
    /// TapTap Android 平台相关导出流程
    /// </summary>
    public class TapTapAndroidBuildStep : AndroidBaseBuildStep
    {
        public override bool IsEnabled {
            get {
                // 是否海外
                if (FunnyConfig.Instance.isMainland)
                {
                    return FunnyConfig.Instance.TapTap.Enable;
                }
                else {
                    return false;
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
                                    if (aar.Name.Equals("TapBootstrap_3.18.6.aar")
                                    || aar.Name.Equals("TapCommon_3.18.6.aar")
                                    || aar.Name.Equals("TapLogin_3.18.6.aar")
                                    || aar.Name.Equals("funny-sdk-taptap.aar")
                                    || aar.Name.Equals("AntiAddiction_3.18.6.aar")
                                    || aar.Name.Equals("AntiAddictionUI_3.18.6.aar"))
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
            depNode.AppendContentNode("implementation 'cn.leancloud:storage-android:8.2.12'");
            depNode.AppendContentNode("implementation(name: 'TapCommon_3.18.6', ext:'aar')");
            depNode.AppendContentNode("implementation(name: 'TapLogin_3.18.6', ext:'aar')");
            depNode.AppendContentNode("implementation(name: 'TapBootstrap_3.18.6', ext:'aar')");
            depNode.AppendContentNode("implementation(name: 'funny-sdk-taptap', ext:'aar')");
        }


        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement clientID = stringsXML.CreateElement("string");
            clientID.SetAttribute("name", "taptap_client_id");
            clientID.InnerText = FunnyConfig.Instance.TapTap.clientID;
            resources.AppendChild(clientID);

            XmlElement clientToken = stringsXML.CreateElement("string");
            clientToken.SetAttribute("name", "taptap_client_token");
            clientToken.InnerText = FunnyConfig.Instance.TapTap.clientToken;
            resources.AppendChild(clientToken);

            XmlElement serverUrl = stringsXML.CreateElement("string");
            serverUrl.SetAttribute("name", "taptap_server_url");
            serverUrl.InnerText = FunnyConfig.Instance.TapTap.serverURL;
            resources.AppendChild(serverUrl);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement clientIdNode = manifestXML.CreateElement("meta-data");
            clientIdNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.taptap.ClientID");
            clientIdNode.SetAttribute("value", NamespaceURI, "@string/taptap_client_id");
            applicationNode.AppendChild(clientIdNode);

            XmlElement clientTokenNode = manifestXML.CreateElement("meta-data");
            clientTokenNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.taptap.ClientToken");
            clientTokenNode.SetAttribute("value", NamespaceURI, "@string/taptap_client_token");
            applicationNode.AppendChild(clientTokenNode);

            XmlElement serverUrlNode = manifestXML.CreateElement("meta-data");
            serverUrlNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.taptap.ServerUrl");
            serverUrlNode.SetAttribute("value", NamespaceURI, "@string/taptap_server_url");
            applicationNode.AppendChild(serverUrlNode);

            // 篝火资格验证开关
            XmlElement channelNode = manifestXML.CreateElement("meta-data");
            channelNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.taptap.isBonFire");
            channelNode.SetAttribute("value", NamespaceURI, FunnyConfig.Instance.TapTap.isBonfire.ToString());
            applicationNode.AppendChild(channelNode);

            // Tap 小号测试开关
            XmlElement bateNode = manifestXML.CreateElement("meta-data");
            bateNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.taptap.isTapBeta");
            bateNode.SetAttribute("value", NamespaceURI, FunnyConfig.Instance.TapTap.isTapBeta.ToString());
            applicationNode.AppendChild(bateNode);

        }

    }
}

#endif