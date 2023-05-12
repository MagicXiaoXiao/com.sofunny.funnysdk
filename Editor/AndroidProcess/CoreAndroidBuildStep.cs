
#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;

namespace SoFunny.FunnySDK.Editor {

    /// <summary>
    /// 核心导出流程类
    /// </summary>
    public class CoreAndroidBuildStep: AndroidBaseBuildStep
    {
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
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.3.11'");
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-android:1.3.9'");
            depNode.AppendContentNode("implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-core:1.3.9'");
            depNode.AppendContentNode("implementation 'com.google.android.gms:play-services-ads-identifier:18.0.1'");
            depNode.AppendContentNode("implementation 'androidx.constraintlayout:constraintlayout:2.1.4'");
        }


        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            // 获取 resources 节点
            var resources = stringsXML.SelectSingleNode("resources");

            // 创建子节点
            XmlElement mainlandValue = stringsXML.CreateElement("string");
            mainlandValue.SetAttribute("name", "funny_sdk_mainland");
            mainlandValue.InnerText = FunnyConfig.Instance.isMainland ? "true" : "false";
            // 加入 resources 节点
            resources.AppendChild(mainlandValue);

            XmlElement appIDValue = stringsXML.CreateElement("string");
            appIDValue.SetAttribute("name", "funny_sdk_app_id");
            appIDValue.InnerText = FunnyConfig.Instance.appID;
            resources.AppendChild(appIDValue);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");
            
            XmlElement funnyAppIdNode = manifestXML.CreateElement("meta-data");
            funnyAppIdNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.FunnyAppId");
            Debug.Log("FunnyAppId: " + FunnyConfig.Instance.appID);
            funnyAppIdNode.SetAttribute("value", NamespaceURI, "@string/funny_sdk_app_id");
            applicationNode.AppendChild(funnyAppIdNode);

            XmlElement channelNode = manifestXML.CreateElement("meta-data");
            channelNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.Mainland");
            channelNode.SetAttribute("value", NamespaceURI, "@string/funny_sdk_mainland");
            applicationNode.AppendChild(channelNode);
        }

        public override void OnProcessLauncherGradle(GradleConfig gradle)
        {
            var androidNode = gradle.ROOT.FindChildNodeByName("android");
            string contentValue = File.ReadAllText(Path.Combine("Packages/com.sofunny.funnysdk/Editor/AndroidAppendExport.gradle"));
            androidNode.AppendContentNode(contentValue);

            var packagingOptions = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("packagingOptions");
            packagingOptions.AppendContentNode("exclude 'META-INF/DEPENDENCIES'");
        }

    }
}

#endif
