#if UNITY_ANDROID

using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SoFunny.FunnySDK.Editor
{
    public class TwitterAndroidBuildStep : AndroidBaseBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled
        {
            get
            {
                if (Config.IsMainland)
                {
                    return false;
                }
                else
                {
                    return Config.Twitter.Enable;
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
                                    return aar.Name.Equals("funny-sdk-twitter.aar");
                                });

            return allAARFiles.ToArray();
        }

        public override void OnProcessUnityLibraryGradle(GradleConfig gradle)
        {
            var depNode = gradle.ROOT.FindChildNodeByName("dependencies");

            depNode.AppendContentNode("implementation 'com.twitter.sdk.android:twitter-core:3.1.1'");
            depNode.AppendContentNode("implementation(name: 'funny-sdk-twitter', ext:'aar')");
        }

        public override void OnProcessUnityLibraryStrings(XmlDocument stringsXML)
        {
            var resources = stringsXML.SelectSingleNode("resources");

            XmlElement twitterAppID = stringsXML.CreateElement("string");
            twitterAppID.SetAttribute("name", "twitter_app_id");
            twitterAppID.InnerText = Config.Twitter.consumerKey;
            resources.AppendChild(twitterAppID);

            XmlElement twitterSecret = stringsXML.CreateElement("string");
            twitterSecret.SetAttribute("name", "twitter_secret");
            twitterSecret.InnerText = Config.Twitter.consumerSecret;
            resources.AppendChild(twitterSecret);

        }

        public override void OnProcessUnityLibraryManifest(XmlDocument manifestXML)
        {
            var rootNode = manifestXML.DocumentElement;
            var applicationNode = rootNode.SelectSingleNode("application");

            XmlElement twitterIdNode = manifestXML.CreateElement("meta-data");
            twitterIdNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.TwitterAppId");
            twitterIdNode.SetAttribute("value", NamespaceURI, "@string/twitter_app_id");
            applicationNode.AppendChild(twitterIdNode);

            XmlElement twitterSecretNode = manifestXML.CreateElement("meta-data");
            twitterSecretNode.SetAttribute("name", NamespaceURI, "com.xmfunny.funnysdk.TwitterSecret");
            twitterSecretNode.SetAttribute("value", NamespaceURI, "@string/twitter_secret");
            applicationNode.AppendChild(twitterSecretNode);

        }

    }
}

#endif