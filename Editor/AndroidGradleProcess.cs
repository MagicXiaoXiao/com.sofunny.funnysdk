#if UNITY_ANDROID

using UnityEngine;
using UnityEditor.Android;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace SoFunny.FunnySDK.Editor {

    partial class AndroidGradleProcess //: IPostGenerateGradleAndroidProject
    {

        //[MenuItem("SoFunnySDK/AndroidTest")]
        //public static void TestMethod2()
        //{
        //    // 预留测试脚本作用
        //    //var steps = new List<AndroidBaseBuildStep>();
        //    //var funnyBuildStepTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
        //    //                          from type in assembly.GetTypes()
        //    //                          where typeof(AndroidBaseBuildStep).IsAssignableFrom(type) && type != typeof(AndroidBaseBuildStep)
        //    //                          select type;

        //    //foreach (var item in funnyBuildStepTypes)
        //    //{
        //    //    Debug.Log($"name = {item.Name}");
        //    //    var step = (AndroidBaseBuildStep)Activator.CreateInstance(item);
        //    //    Debug.Log($"IsEnabled = {step.IsEnabled}");
        //    //    if (step.IsEnabled)
        //    //    {
        //    //        steps.Add(step);
        //    //    }
        //    //}

        //    //Debug.Log($"count = {steps.Count}");
        //}

        public int callbackOrder {
            get {
                return 777;
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path) {

            FunnyConfig.Instance.SyncToConfig();
            //Android 导出脚本执行
            Start(path);

        }
    }


    partial class AndroidGradleProcess {

        private const string FACEBOOK_SDK = "implementation ('com.facebook.android:facebook-android-sdk:15.0.0')";
        private const string GOOGLEPLAY_SDK = "implementation 'com.google.android.gms:play-services-auth:20.0.1'";
        private const string TWITTER_SDK = "implementation 'com.twitter.sdk.android:twitter-core:3.1.1'";
        private const string QQ_SDK = "implementation 'com.tencent.tauth:qqopensdk:3.52.0'";
        private const string WECHAT_SDK = "implementation 'com.tencent.mm.opensdk:wechat-sdk-android:6.8.0'";
        private const string TAPTAP_SDK = "implementation 'cn.leancloud:storage-android:8.2.12'";
        private static string AAR_ORIGIN_PATH => Path.Combine(FunnyConfig.Instance._defaultPluginPath, "ExtPackage/Android");

        internal static void Start(string projectPath) {
            prepareAARFile(projectPath);
            SetupGradleProperties(projectPath);
            SetupBuildGradle(projectPath);
            SetupBuildLauncherGradle(projectPath);
            SetupStringsXMLFile(projectPath);
            SetupManifestFile(projectPath);
        }

        private static void prepareAARFile(string projectPath)
        {
            var allAARFiles = Directory.GetFiles(AAR_ORIGIN_PATH)
                                .Where((dirPath) => {
                                    return Path.GetExtension(dirPath) == ".aar";
                                })
                                .Select((dirPath) => {
                                    return new FileInfo(dirPath);
                                })
                                .Where((aar) => {
                                    if (FunnyConfig.Instance.Facebook.Enable && aar.Name.Equals("funny-sdk-facebook.aar"))
                                    {
                                        return true;
                                    }
                                    else if (FunnyConfig.Instance.Google.Enable && aar.Name.Equals("funny-sdk-googleplay.aar"))
                                    {
                                        return true;
                                    }
                                    else if (FunnyConfig.Instance.Twitter.Enable && aar.Name.Equals("funny-sdk-twitter.aar")){
                                        return true;
                                    }
                                    else if (FunnyConfig.Instance.TapTap.Enable && (aar.Name.Equals("TapBootstrap_3.18.6.aar")
                                    || aar.Name.Equals("TapCommon_3.18.6.aar")
                                    || aar.Name.Equals("TapLogin_3.18.6.aar")
                                    || aar.Name.Equals("funny-sdk-taptap.aar"))) {
                                        return true;
                                    }
                                    else if (FunnyConfig.Instance.WeChat.Enable && aar.Name.Equals("funny-sdk-wechat.aar")) {
                                        return true;
                                    }
                                    else if (FunnyConfig.Instance.QQ.Enable && aar.Name.Equals("funny-sdk-qq.aar")) {
                                        return true;
                                    }
                                    return false;
                                });
            var libDir = Path.Combine(projectPath, "libs");

            // Add SoFunny aars
            foreach (var aarFile in allAARFiles)
            {
                Debug.Log("aar path: " + aarFile.FullName + " lib path: " + libDir);
                FunnyUtils.Copy(aarFile.FullName, Path.Combine(libDir, aarFile.Name));
            }

        }

        private static void SetupGradleProperties(string projectPath) {
            DirectoryInfo androidPath = new DirectoryInfo(projectPath);
            // 获取 gradle.properties 文件
            var files = androidPath.Parent.GetFiles("gradle.properties");

            // 不存在则创建
            if (files.Length <= 0) {
                var propPath = Path.Combine(androidPath.Parent.FullName, "gradle.properties");
                var propFile = File.Create(propPath);
                propFile.Flush();
                propFile.Close();
            }
            // 存在则获取首个文件并转为 FileInfo 对象
            var file = files.First();

            // 创建临时文件对象
            var tempFile = new FileInfo(projectPath + "/properties.temp");

            // 创建临时解析数据对象
            Dictionary<string, string> properties = new Dictionary<string, string>();

            // 解析文件内容并添加到解析对象
            using (StreamReader sr = file.OpenText()) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    var props = line.Split('=');
                    if (props.Length >= 2) {
                        properties.Add(props[0], props[1]);
                    }
                }
            }

            // 配置相关参数
            properties["android.useAndroidX"] = "true";
            properties["android.enableJetifier"] = "true";

            // 写入临时配置文件
            using (StreamWriter sw = tempFile.CreateText()) {

                foreach (var item in properties) {
                    sw.WriteLine($"{item.Key}={item.Value}");
                }
            }

            // 删除源文件
            file.Delete();
            // 移动临时文件到目标位置
            tempFile.MoveTo(file.FullName);

        }

        private static void SetupBuildGradle(string projectPath) {
            DirectoryInfo androidPath = new DirectoryInfo(projectPath);
            var files = androidPath.GetFiles("build.gradle");
            var file = files.First();

            var gradle = new GradleConfig(file.FullName);
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

            if (FunnyConfig.Instance.isMainland) {
                if (FunnyConfig.Instance.WeChat.Enable) {
                    depNode.AppendContentNode(WECHAT_SDK);
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-wechat', ext:'aar')");
                }

                if (FunnyConfig.Instance.QQ.Enable) {
                    depNode.AppendContentNode(QQ_SDK);
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-qq', ext:'aar')");
                }
                if (FunnyConfig.Instance.TapTap.Enable) {
                    depNode.AppendContentNode(TAPTAP_SDK);
                    depNode.AppendContentNode("implementation(name: 'TapCommon_3.18.6', ext:'aar')");
                    depNode.AppendContentNode("implementation(name: 'TapLogin_3.18.6', ext:'aar')");
                    depNode.AppendContentNode("implementation(name: 'TapBootstrap_3.18.6', ext:'aar')");
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-taptap', ext:'aar')");
                }
            }
            else {

                if (FunnyConfig.Instance.Facebook.Enable) {
                    var fbNode = new GradleNode(FACEBOOK_SDK);
                    fbNode.AppendContentNode("exclude group: 'androidx.test', module: 'core'");
                    fbNode.AppendContentNode("exclude group: 'androidx.test', module: 'monitor'");
                    depNode.AppendChildNode(fbNode);
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-facebook', ext:'aar')");
                }

                if (FunnyConfig.Instance.Google.Enable) {
                    depNode.AppendContentNode(GOOGLEPLAY_SDK);
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-googleplay', ext:'aar')");
                    depNode.AppendContentNode("implementation 'com.google.api-client:google-api-client:1.31.1'");
                    depNode.AppendContentNode("implementation 'com.google.api-client:google-api-client-android:1.31.0'");
                    depNode.AppendContentNode("implementation 'com.google.apis:google-api-services-people:v1-rev20201117-1.31.0'");
                }

                if (FunnyConfig.Instance.Twitter.Enable) {
                    depNode.AppendContentNode(TWITTER_SDK);
                    depNode.AppendContentNode("implementation(name: 'funny-sdk-twitter', ext:'aar')");
                }

            }

            gradle.Save();
        }

        private static void SetupBuildLauncherGradle(string projectPath) {
            DirectoryInfo unityLibraryPath = new DirectoryInfo(projectPath);
            var launcherDir = unityLibraryPath.Parent.GetDirectories().Where(x => x.Name == "launcher").First();
            var gradleFile = launcherDir.GetFiles("build.gradle").First();

            var gradle = new GradleConfig(gradleFile.FullName);
     
            var androidNode = gradle.ROOT.FindChildNodeByName("android");
            string contentValue = File.ReadAllText(Path.Combine("Packages/com.sofunny.funnysdk/Editor/AndroidAppendExport.gradle"));
            androidNode.AppendContentNode(contentValue);

            var packagingOptions = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("packagingOptions");
            packagingOptions.AppendContentNode("exclude 'META-INF/DEPENDENCIES'");      
      
            if (FunnyConfig.Instance.WeChat.Enable) {
                var defaultConfig = gradle.ROOT.FindChildNodeByName("android").FindChildNodeByName("defaultConfig");
                defaultConfig.AppendContentNode("manifestPlaceholders = [\nwechat_package: applicationId\n]");
            }
            gradle.Save();
        }

        private static void SetupStringsXMLFile(string projectPath) {
            var stringsPath = Path.Combine(projectPath, "src/main/res/values/strings.xml");
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            
            XmlElement resources = doc.CreateElement(string.Empty, "resources", string.Empty);
            doc.AppendChild(resources);
            SetMainland();
            SetFunnyAppId();
            if (FunnyConfig.Instance.isMainland) {
                SetWeChatXML();
                SetQQXML();
                SetTapTapXML();
            }
            else {
                if (FunnyConfig.Instance.Facebook.Enable){
                    SetFacebookXML();
                }
                if (FunnyConfig.Instance.Google.Enable){
                    SetGooglePlayXML();
                }
                if (FunnyConfig.Instance.Twitter.Enable){
                    SetTwitterXML();
                }
            }

            void SetMainland() {
                XmlElement mainlandValue = doc.CreateElement("string");
                mainlandValue.SetAttribute("name", "funny_sdk_mainland");
                mainlandValue.InnerText = FunnyConfig.Instance.isMainland ? "true" : "false";
                resources.AppendChild(mainlandValue);
            }

             void SetFunnyAppId() {
                XmlElement mainlandValue = doc.CreateElement("string");
                mainlandValue.SetAttribute("name", "funny_sdk_app_id");
                mainlandValue.InnerText = FunnyConfig.Instance.appID;
                resources.AppendChild(mainlandValue);
            }

            void SetFacebookXML() {
                XmlElement facebookAppID = doc.CreateElement("string");
                facebookAppID.SetAttribute("name", "facebook_app_id");
                facebookAppID.InnerText = FunnyConfig.Instance.Facebook.appID;
                resources.AppendChild(facebookAppID);

                XmlElement facebookScheme = doc.CreateElement("string");
                facebookScheme.SetAttribute("name", "fb_login_protocol_scheme");
                facebookScheme.InnerText = "fb" + FunnyConfig.Instance.Facebook.appID;
                resources.AppendChild(facebookScheme);

                XmlElement facebookClient = doc.CreateElement("string");
                facebookClient.SetAttribute("name", "facebook_client_token");
                facebookClient.InnerText = FunnyConfig.Instance.Facebook.clientToken;
                resources.AppendChild(facebookClient);
            }

            void SetTwitterXML() {
                XmlElement twitterAppID = doc.CreateElement("string");
                twitterAppID.SetAttribute("name", "twitter_app_id");
                twitterAppID.InnerText = FunnyConfig.Instance.Twitter.consumerKey;
                resources.AppendChild(twitterAppID);

                XmlElement twitterSecret = doc.CreateElement("string");
                twitterSecret.SetAttribute("name", "twitter_secret");
                twitterSecret.InnerText = FunnyConfig.Instance.Twitter.consumerSecret;
                resources.AppendChild(twitterSecret);
            }

            void SetGooglePlayXML() {
                XmlElement googleAppID = doc.CreateElement("string");
                googleAppID.SetAttribute("name", "google_play_app_id");
                googleAppID.InnerText = FunnyConfig.Instance.Google.idToken;
                resources.AppendChild(googleAppID);
            }

            void SetWeChatXML() {
                XmlElement wechatAppID = doc.CreateElement("string");
                wechatAppID.SetAttribute("name", "wechat_app_id");
                wechatAppID.InnerText = FunnyConfig.Instance.WeChat.appID;
                resources.AppendChild(wechatAppID);
            }

            void SetQQXML() {
                XmlElement qqAppID = doc.CreateElement("string");
                qqAppID.SetAttribute("name", "qq_app_id");
                qqAppID.InnerText = FunnyConfig.Instance.QQ.appID;
                resources.AppendChild(qqAppID);
            }
            void SetTapTapXML() {
                XmlElement clientID = doc.CreateElement("string");
                clientID.SetAttribute("name", "taptap_client_id");
                clientID.InnerText = FunnyConfig.Instance.TapTap.clientID;
                resources.AppendChild(clientID);

                XmlElement clientToken = doc.CreateElement("string");
                clientToken.SetAttribute("name", "taptap_client_token");
                clientToken.InnerText = FunnyConfig.Instance.TapTap.clientToken;
                resources.AppendChild(clientToken);

                XmlElement serverUrl = doc.CreateElement("string");
                serverUrl.SetAttribute("name", "taptap_server_url");
                serverUrl.InnerText = FunnyConfig.Instance.TapTap.serverURL;
                resources.AppendChild(serverUrl);
            }
            doc.Save(stringsPath);
        }

        private static void SetupManifestFile(string projectPath) {
            const string namespaceURI = "http://schemas.android.com/apk/res/android";
            var manifestPath = Path.Combine(projectPath, "src/main/AndroidManifest.xml");

            if (File.Exists(manifestPath)) {

                XmlDocument doc = new XmlDocument();
                doc.Load(manifestPath);
                var rootNode = doc.DocumentElement;
                var applicationNode = rootNode.SelectSingleNode("application");

                SetFunnyAppId();
                SetMainlandChannel();
                //SetBonfire();

                if (FunnyConfig.Instance.isMainland) {
                    SetWeChat();
                    SetQQ();
                    SetTapTap();
                }
                else {
                    if (FunnyConfig.Instance.Facebook.Enable)
                    {
                        SetFacebook();
                        SetFaceBookCustomTabActivity();
                    }
                    if (FunnyConfig.Instance.Google.Enable)
                    {
                        SetGooglePlay();
                    }
                    if (FunnyConfig.Instance.Twitter.Enable)
                    {
                        SetTwitter();
                    }
                }


                void SetFunnyAppId()
                {
                    XmlElement funnyAppIdNode = doc.CreateElement("meta-data");
                    funnyAppIdNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.FunnyAppId");
                    Debug.Log("FunnyAppId: " + FunnyConfig.Instance.appID);
                    funnyAppIdNode.SetAttribute("value", namespaceURI, "@string/funny_sdk_app_id");
                    applicationNode.AppendChild(funnyAppIdNode);
                }


                void SetMainlandChannel() {
                    XmlElement channelNode = doc.CreateElement("meta-data");
                    channelNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.Mainland");
                    channelNode.SetAttribute("value", namespaceURI, "@string/funny_sdk_mainland");
                    applicationNode.AppendChild(channelNode);
                }

                //void SetBonfire() {
                //    XmlElement channelNode = doc.CreateElement("meta-data");
                //    channelNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.isBonFire");
                //    channelNode.SetAttribute("value", namespaceURI, FunnyConfig.Instance.isBonfire.ToString());
                //    applicationNode.AppendChild(channelNode);
                //}

                void SetFacebook() {
                    XmlElement facebookAppIDNode = doc.CreateElement("meta-data");
                    facebookAppIDNode.SetAttribute("name", namespaceURI, "com.facebook.sdk.ApplicationId");
                    facebookAppIDNode.SetAttribute("value", namespaceURI, "@string/facebook_app_id");
                    applicationNode.AppendChild(facebookAppIDNode);

                    XmlElement facebookClientTokenNode = doc.CreateElement("meta-data");
                    facebookClientTokenNode.SetAttribute("name", namespaceURI, "com.facebook.sdk.ClientToken");
                    facebookClientTokenNode.SetAttribute("value", namespaceURI, "@string/facebook_client_token");
                    applicationNode.AppendChild(facebookClientTokenNode);

                    var trackValue = FunnyConfig.Instance.Facebook.trackEnable ? "true" : "false";

                    XmlElement facebookTrackNode = doc.CreateElement("meta-data");
                    facebookTrackNode.SetAttribute("name", namespaceURI, "com.facebook.sdk.AutoLogAppEventsEnabled");
                    facebookTrackNode.SetAttribute("value", namespaceURI, trackValue);
                    applicationNode.AppendChild(facebookTrackNode);

                    XmlElement facebookADVNode = doc.CreateElement("meta-data");
                    facebookADVNode.SetAttribute("name", namespaceURI, "com.facebook.sdk.AdvertiserIDCollectionEnabled");
                    facebookADVNode.SetAttribute("value", namespaceURI, trackValue);
                    applicationNode.AppendChild(facebookADVNode);

                    XmlElement facebookAutoInitNode = doc.CreateElement("meta-data");
                    facebookAutoInitNode.SetAttribute("name", namespaceURI, "com.facebook.sdk.AutoInitEnabled");
                    facebookAutoInitNode.SetAttribute("value", namespaceURI, trackValue);
                    applicationNode.AppendChild(facebookAutoInitNode);

                }

                void SetGooglePlay() {
                    XmlElement googleNode = doc.CreateElement("meta-data");
                    googleNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.GooglePlayAppId");
                    googleNode.SetAttribute("value", namespaceURI, "@string/google_play_app_id");
                    applicationNode.AppendChild(googleNode);
                }

                void SetTwitter() {
                    XmlElement twitterIdNode = doc.CreateElement("meta-data");
                    twitterIdNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.TwitterAppId");
                    twitterIdNode.SetAttribute("value", namespaceURI, "@string/twitter_app_id");
                    applicationNode.AppendChild(twitterIdNode);

                    XmlElement twitterSecretNode = doc.CreateElement("meta-data");
                    twitterSecretNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.TwitterSecret");
                    twitterSecretNode.SetAttribute("value", namespaceURI, "@string/twitter_secret");
                    applicationNode.AppendChild(twitterSecretNode);
                }

                void SetWeChat() {
                    XmlElement wechatNode = doc.CreateElement("meta-data");
                    wechatNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.WeChatAppId");
                    wechatNode.SetAttribute("value", namespaceURI, "@string/wechat_app_id");
                    applicationNode.AppendChild(wechatNode);
                }

                void SetQQ() {
                    XmlElement qqNode = doc.CreateElement("meta-data");
                    qqNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.QQAppId");
                    qqNode.SetAttribute("value", namespaceURI, "@string/qq_app_id");
                    applicationNode.AppendChild(qqNode);

                    XmlElement orgLegacyNode = doc.CreateElement("uses-library");
                    orgLegacyNode.SetAttribute("name", namespaceURI, "org.apache.http.legacy");
                    orgLegacyNode.SetAttribute("required", namespaceURI, "false");
                    applicationNode.AppendChild(orgLegacyNode);
                }

                void SetTapTap() {
                    XmlElement clientIdNode = doc.CreateElement("meta-data");
                    clientIdNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.taptap.ClientID");
                    clientIdNode.SetAttribute("value", namespaceURI, "@string/taptap_client_id");
                    applicationNode.AppendChild(clientIdNode);

                    XmlElement clientTokenNode = doc.CreateElement("meta-data");
                    clientTokenNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.taptap.ClientToken");
                    clientTokenNode.SetAttribute("value", namespaceURI, "@string/taptap_client_token");
                    applicationNode.AppendChild(clientTokenNode);

                    XmlElement serverUrlNode = doc.CreateElement("meta-data");
                    serverUrlNode.SetAttribute("name", namespaceURI, "com.xmfunny.funnysdk.taptap.ServerUrl");
                    serverUrlNode.SetAttribute("value", namespaceURI, "@string/taptap_server_url");
                    applicationNode.AppendChild(serverUrlNode);
                }

                void SetFaceBookCustomTabActivity()
                {
                    XmlElement faceBookTabActivityNode = doc.CreateElement("activity");
                    faceBookTabActivityNode.SetAttribute("name", namespaceURI, "com.facebook.CustomTabActivity");
                    faceBookTabActivityNode.SetAttribute("exported", namespaceURI, "true");

                    XmlElement intentFilterNode = doc.CreateElement("intent-filter");

                    XmlElement actionNode = doc.CreateElement("action");
                    actionNode.SetAttribute("name", namespaceURI, "android.intent.action.VIEW");

                    XmlElement defaultCategoryNode = doc.CreateElement("category");
                    defaultCategoryNode.SetAttribute("name", namespaceURI, "android.intent.category.DEFAULT");

                    XmlElement browsableCategoryNode = doc.CreateElement("category");
                    browsableCategoryNode.SetAttribute("name", namespaceURI, "android.intent.category.BROWSABLE");

                    XmlElement dataNode = doc.CreateElement("data");
                    dataNode.SetAttribute("scheme", namespaceURI, "@string/fb_login_protocol_scheme");

                    intentFilterNode.AppendChild(actionNode);
                    intentFilterNode.AppendChild(defaultCategoryNode);
                    intentFilterNode.AppendChild(browsableCategoryNode);
                    intentFilterNode.AppendChild(dataNode);

                    faceBookTabActivityNode.AppendChild(intentFilterNode);

                    applicationNode.AppendChild(faceBookTabActivityNode);
                }
                doc.Save(manifestPath);
            }
            else {
                Debug.LogError($"不存在路径文件 -> {manifestPath}");
            }
        }
    }

}

#endif