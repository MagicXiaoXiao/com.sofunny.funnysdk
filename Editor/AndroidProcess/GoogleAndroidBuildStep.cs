
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
        private Google googleConfig;
        private bool _enable = false;

        public override bool IsEnabled => _enable;

        internal override void OnInitConfig(Configuration.Android config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (editorConfig.IsMainland) return;

                googleConfig = Google.Create(editorConfig.Google.AndroidClientID);
                googleConfig.SetGooglePlayGames(editorConfig.Google.GmsGamesEnable, editorConfig.Google.gmsGamesAppId);

                _enable = googleConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                googleConfig = config.SetupGoogle();

                if (initConfig.Env == FunnyEnv.Overseas)
                {
                    _enable = googleConfig.Enable;
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

            if (googleConfig.EnableGooglePlayGames)
            {
                depNode.AppendContentNode("implementation 'com.google.android.gms:play-services-games-v2:19.0.0'");
            }
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            // 获取 resources 节点
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement googleAppID = stringsXML.CreateElement("string");
            googleAppID.SetAttribute("name", "google_play_app_id");
            googleAppID.InnerText = googleConfig.ClientID;
            resources.AppendChild(googleAppID);

            if (googleConfig.EnableGooglePlayGames)
            {
                XmlElement gmsGamesAppIDValue = stringsXML.CreateElement("string");
                gmsGamesAppIDValue.SetAttribute("name", "funny_sdk_gms_games.app_id");
                gmsGamesAppIDValue.InnerText = googleConfig.PlayGamesAppID;
                resources.AppendChild(gmsGamesAppIDValue);
            }
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement googleNode = manifestXML.CreateElement("meta-data");
            googleNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.GooglePlayAppId");
            googleNode.SetAttribute("value", NamespaceURI, "@string/google_play_app_id");
            applicationNode.AppendChild(googleNode);

            if (googleConfig.EnableGooglePlayGames)
            {
                XmlElement googleGMSGamesIdNode = manifestXML.CreateElement("meta-data");
                googleGMSGamesIdNode.SetAttribute("name", NamespaceURI, "com.google.android.gms.games.APP_ID");
                googleGMSGamesIdNode.SetAttribute("value", NamespaceURI, "@string/funny_sdk_gms_games.app_id");
                applicationNode.AppendChild(googleGMSGamesIdNode);

                XmlElement gmsVersion = manifestXML.CreateElement("meta-data");
                gmsVersion.SetAttribute("name", NamespaceURI, "com.google.android.gms.version");
                gmsVersion.SetAttribute("value", NamespaceURI, "@integer/google_play_services_version");
                applicationNode.AppendChild(gmsVersion);
            }
        }

    }
}

#endif