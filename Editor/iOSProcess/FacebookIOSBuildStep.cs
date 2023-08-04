#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class FacebookIOSBuildStep : FunnyXcodeBuildStep
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
                    return Config.Facebook.Enable;
                }
            }
        }

        public override DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var allXCFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) =>
                                 {
                                     return Path.GetExtension(dirPath) == FUNNY_FRAMEWORK_EXTENSION;
                                 })
                                 .Select((dirPath) =>
                                 {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) =>
                                 {
                                     return framework.Name == "FFacebookOpenAPI.framework";
                                 });

            return allXCFramework.ToArray();
        }

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var facebookURLScheme = urlSchemeArray.AddDict();
            facebookURLScheme.SetString("CFBundleTypeRole", "Editor");
            facebookURLScheme.SetString("CFBundleURLName", "FACEBOOK SDK");
            var fbSchemes = facebookURLScheme.CreateArray("CFBundleURLSchemes");
            fbSchemes.AddString($"fb{Config.Facebook.appID}");

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("fbapi");
            queriesSchemes.AddString("fbauth");
            queriesSchemes.AddString("fbauth2");
            queriesSchemes.AddString("fbshareextension");
            queriesSchemes.AddString("fb-messenger-share-api");

            rootDict.SetBoolean("FacebookAutoLogAppEventsEnabled", false);
            rootDict.SetBoolean("FacebookAdvertiserIDCollectionEnabled", false);
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_FACEBOOK_APPID", Config.Facebook.appID);
            sofunnyDict.SetString("FUNNY_FACEBOOK_CLIENTTOKEN", Config.Facebook.clientToken);
            sofunnyDict.SetBoolean("FUNNY_FACEBOOK_TRACK", Config.Facebook.trackEnable);
        }

    }
}

#endif