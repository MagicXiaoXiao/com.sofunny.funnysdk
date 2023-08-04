
#if UNITY_ANDROID

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace SoFunny.FunnySDK.Editor
{

    /// <summary>
    /// 谷歌相关配置流程脚本
    /// </summary>
    public class GoogleAndroidBuildStep : AndroidBaseBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled
        {
            get
            {
                // 是否海外
                if (Config.IsMainland)
                {
                    return false;
                }
                else
                {
                    return Config.Google.Enable;
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
                                    return aar.Name.Equals("funny-sdk-googleplay.aar");
                                });

            return allAARFiles.ToArray();
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");
            depNode.AppendContentNode("implementation 'com.google.android.gms:play-services-auth:20.0.1'");
            depNode.AppendContentNode("implementation(name: 'funny-sdk-googleplay', ext:'aar')");
            depNode.AppendContentNode("implementation 'com.google.api-client:google-api-client:1.31.1'");
            depNode.AppendContentNode("implementation 'com.google.api-client:google-api-client-android:1.31.0'");
            depNode.AppendContentNode("implementation 'com.google.apis:google-api-services-people:v1-rev20201117-1.31.0'");
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            // 获取 resources 节点
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement googleAppID = stringsXML.CreateElement("string");
            googleAppID.SetAttribute("name", "google_play_app_id");
            googleAppID.InnerText = Config.Google.idToken;
            resources.AppendChild(googleAppID);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement googleNode = manifestXML.CreateElement("meta-data");
            googleNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.GooglePlayAppId");
            googleNode.SetAttribute("value", NamespaceURI, "@string/google_play_app_id");
            applicationNode.AppendChild(googleNode);
        }

    }
}

#endif