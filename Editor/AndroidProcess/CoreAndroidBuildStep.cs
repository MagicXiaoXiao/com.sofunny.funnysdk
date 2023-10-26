﻿
#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Linq;

namespace SoFunny.FunnySDK.Editor
{

    /// <summary>
    /// 核心导出流程类
    /// </summary>
    public class CoreAndroidBuildStep : AndroidBaseBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled => true;

        public override void OnProcessGradleProperties(Dictionary<string, string> properties)
        {
            // 配置相关参数
            properties["android.useAndroidX"] = "true";
            properties["android.enableJetifier"] = "true";
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");
            depNode.AppendContentNode("implementation 'androidx.annotation:annotation: 1.1.0'");
            depNode.AppendContentNode("implementation 'androidx.appcompat:appcompat:1.1.0'");
            depNode.AppendContentNode("implementation 'androidx.core:core-ktx:1.2.0'");
            depNode.AppendContentNode("implementation 'com.google.code.gson:gson:2.8.5'");

            var madgagNode = new GradleNode("implementation('com.madgag.spongycastle:prov:1.58.0.0')");
            madgagNode.AppendContentNode("exclude group: 'junit', module: 'junit'");
            depNode.AppendChildNode(madgagNode);

            var jjwtNode = new GradleNode("implementation('io.jsonwebtoken:jjwt-orgjson:0.11.1')");
            jjwtNode.AppendContentNode("exclude group: 'org.json', module: 'json'");
            depNode.AppendChildNode(jjwtNode);

            depNode.AppendContentNode("implementation 'com.google.android.material:material:1.0.0'");
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.4.32'");
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-android:1.3.9'");
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-core:1.3.9'");
            depNode.AppendContentNode("implementation 'com.google.android.gms:play-services-ads-identifier:18.0.1'");
            depNode.AppendContentNode("implementation 'androidx.constraintlayout:constraintlayout:2.1.4'");

            depNode.AppendContentNode("implementation 'com.squareup.retrofit2:retrofit:2.9.0'");
            depNode.AppendContentNode("implementation 'com.squareup.okhttp3:okhttp:4.7.2'");
            if (IsDebug)
            {
                depNode.AppendContentNode("implementation 'com.squareup.okhttp3:logging-interceptor:4.7.2'");
            }
            depNode.AppendContentNode("implementation 'com.squareup.retrofit2:converter-gson:2.9.0'");
            //depNode.AppendContentNode("implementation 'com.aliyun.ams:alicloud-android-httpdns:2.3.0'");
            string aarName = IsDebug ? "funny-sdk-debug" : "funny-sdk";
            depNode.AppendContentNode($"implementation(name: '{aarName}', ext:'aar')");
            if (Config.Google.GmsGamesEnable)
            {
                depNode.AppendContentNode("implementation 'com.google.android.gms:play-services-games-v2:19.0.0'");
            }
        }


        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            // 获取 resources 节点
            var resources = stringsXML.SelectSingleNode("resources");

            // 创建子节点
            XmlElement mainlandValue = stringsXML.CreateElement("string");
            mainlandValue.SetAttribute("name", "funny_sdk_mainland");
            mainlandValue.InnerText = Config.IsMainland ? "true" : "false";
            // 加入 resources 节点
            resources.AppendChild(mainlandValue);

            XmlElement appIDValue = stringsXML.CreateElement("string");
            appIDValue.SetAttribute("name", "funny_sdk_app_id");
            appIDValue.InnerText = Config.AppID;
            resources.AppendChild(appIDValue);

            if (Config.Google.GmsGamesEnable)
            {
                XmlElement gmsGamesAppIDValue = stringsXML.CreateElement("string");
                gmsGamesAppIDValue.SetAttribute("name", "funny_sdk_gms_games.app_id");
                gmsGamesAppIDValue.InnerText = Config.Google.gmsGamesAppId;
                resources.AppendChild(gmsGamesAppIDValue);
            }
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement funnyAppIdNode = manifestXML.CreateElement("meta-data");
            funnyAppIdNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.FunnyAppId");
            funnyAppIdNode.SetAttribute("value", NamespaceURI, "@string/funny_sdk_app_id");
            applicationNode.AppendChild(funnyAppIdNode);

            XmlElement channelNode = manifestXML.CreateElement("meta-data");
            channelNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.Mainland");
            channelNode.SetAttribute("value", NamespaceURI, "@string/funny_sdk_mainland");
            applicationNode.AppendChild(channelNode);

            if (Config.Google.GmsGamesEnable)
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

        public override void OnProcessLauncherGradle(GradleConfig gradle)
        {

#if UNITY_2022_2_OR_NEWER
            var androidNode = gradle.ROOT.FindChildNodeByName("android");
            string contentValue = File.ReadAllText(Path.Combine("Packages/com.sofunny.funnysdk/Editor/AndroidAppendExport_2022_2_newer.gradle"));
            androidNode.AppendContentNode(contentValue);
#else
            var androidNode = gradle.ROOT.FindChildNodeByName("android");
            string contentValue = File.ReadAllText(Path.Combine("Packages/com.sofunny.funnysdk/Editor/AndroidAppendExport.gradle"));
            androidNode.AppendContentNode(contentValue);
#endif
            var packagingOptions = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("packagingOptions");
            packagingOptions.AppendContentNode("exclude 'META-INF/DEPENDENCIES'");
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
                                    string matchName = IsDebug ? "funny-sdk-debug.aar" : "funny-sdk.aar";
                                    return aar.Name.Equals(matchName);
                                });

            return allAARFiles.ToArray();
        }


    }
}

#endif
