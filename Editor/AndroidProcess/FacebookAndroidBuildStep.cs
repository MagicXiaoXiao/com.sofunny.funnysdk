
#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    public class FacebookAndroidBuildStep : AndroidBaseBuildStep
    {
        private Facebook facebookConfig;
        private bool _enable = false;

        public override bool IsEnabled => _enable;

        internal override void OnInitConfig(Configuration.Android config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (editorConfig.IsMainland) return;

                facebookConfig = Facebook.Create(editorConfig.Facebook.appID, editorConfig.Facebook.clientToken);
                _enable = facebookConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                facebookConfig = config.SetupFacebook();

                if (initConfig.Env == FunnyEnv.Overseas)
                {
                    _enable = facebookConfig.Enable;
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
                                    return aar.Name.Equals("funny-sdk-facebook.aar");
                                });

            return allAARFiles.ToArray();
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");

            var fbNode = new GradleNode("implementation ('com.facebook.android:facebook-android-sdk:15.0.0')");
            fbNode.AppendContentNode("exclude group: 'androidx.test', module: 'core'");
            fbNode.AppendContentNode("exclude group: 'androidx.test', module: 'monitor'");
            fbNode.AppendContentNode("exclude group: 'org.robolectric'");

            depNode.AppendChildNode(fbNode);
            depNode.AppendContentNode("implementation(name: 'funny-sdk-facebook', ext:'aar')");
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement facebookAppID = stringsXML.CreateElement("string");
            facebookAppID.SetAttribute("name", "facebook_app_id");
            facebookAppID.InnerText = facebookConfig.AppID;
            resources.AppendChild(facebookAppID);

            XmlElement facebookScheme = stringsXML.CreateElement("string");
            facebookScheme.SetAttribute("name", "fb_login_protocol_scheme");
            facebookScheme.InnerText = "fb" + facebookConfig.AppID;
            resources.AppendChild(facebookScheme);

            XmlElement facebookClient = stringsXML.CreateElement("string");
            facebookClient.SetAttribute("name", "facebook_client_token");
            facebookClient.InnerText = facebookConfig.ClientToken;
            resources.AppendChild(facebookClient);
        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement facebookAppIDNode = manifestXML.CreateElement("meta-data");
            facebookAppIDNode.SetAttribute("name", NamespaceURI, "com.facebook.sdk.ApplicationId");
            facebookAppIDNode.SetAttribute("value", NamespaceURI, "@string/facebook_app_id");
            applicationNode.AppendChild(facebookAppIDNode);

            XmlElement facebookClientTokenNode = manifestXML.CreateElement("meta-data");
            facebookClientTokenNode.SetAttribute("name", NamespaceURI, "com.facebook.sdk.ClientToken");
            facebookClientTokenNode.SetAttribute("value", NamespaceURI, "@string/facebook_client_token");
            applicationNode.AppendChild(facebookClientTokenNode);

            var trackValue = facebookConfig.EnableAdvertiserTrack ? "true" : "false";

            XmlElement facebookTrackNode = manifestXML.CreateElement("meta-data");
            facebookTrackNode.SetAttribute("name", NamespaceURI, "com.facebook.sdk.AutoLogAppEventsEnabled");
            facebookTrackNode.SetAttribute("value", NamespaceURI, trackValue);
            applicationNode.AppendChild(facebookTrackNode);

            XmlElement facebookADVNode = manifestXML.CreateElement("meta-data");
            facebookADVNode.SetAttribute("name", NamespaceURI, "com.facebook.sdk.AdvertiserIDCollectionEnabled");
            facebookADVNode.SetAttribute("value", NamespaceURI, trackValue);
            applicationNode.AppendChild(facebookADVNode);

            XmlElement facebookAutoInitNode = manifestXML.CreateElement("meta-data");
            facebookAutoInitNode.SetAttribute("name", NamespaceURI, "com.facebook.sdk.AutoInitEnabled");
            facebookAutoInitNode.SetAttribute("value", NamespaceURI, trackValue);
            applicationNode.AppendChild(facebookAutoInitNode);


            XmlElement faceBookTabActivityNode = manifestXML.CreateElement("activity");
            faceBookTabActivityNode.SetAttribute("name", NamespaceURI, "com.facebook.CustomTabActivity");
            faceBookTabActivityNode.SetAttribute("exported", NamespaceURI, "true");

            XmlElement intentFilterNode = manifestXML.CreateElement("intent-filter");

            XmlElement actionNode = manifestXML.CreateElement("action");
            actionNode.SetAttribute("name", NamespaceURI, "android.intent.action.VIEW");

            XmlElement defaultCategoryNode = manifestXML.CreateElement("category");
            defaultCategoryNode.SetAttribute("name", NamespaceURI, "android.intent.category.DEFAULT");

            XmlElement browsableCategoryNode = manifestXML.CreateElement("category");
            browsableCategoryNode.SetAttribute("name", NamespaceURI, "android.intent.category.BROWSABLE");

            XmlElement dataNode = manifestXML.CreateElement("data");
            dataNode.SetAttribute("scheme", NamespaceURI, "@string/fb_login_protocol_scheme");

            intentFilterNode.AppendChild(actionNode);
            intentFilterNode.AppendChild(defaultCategoryNode);
            intentFilterNode.AppendChild(browsableCategoryNode);
            intentFilterNode.AppendChild(dataNode);

            faceBookTabActivityNode.AppendChild(intentFilterNode);
            applicationNode.AppendChild(faceBookTabActivityNode);
        }

    }
}

#endif