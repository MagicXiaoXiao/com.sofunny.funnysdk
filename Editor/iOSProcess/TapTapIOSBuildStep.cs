#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class TapTapIOSBuildStep : FunnyXcodeBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled
        {
            get
            {
                return Config.IsMainland && Config.TapTap.Enable;
            }
        }

        public override DirectoryInfo[] OnProcessBundles(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var allBundles = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) =>
                                 {
                                     return Path.GetExtension(dirPath) == ".bundle";
                                 })
                                 .Select((dirPath) =>
                                 {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) =>
                                 {
                                     return framework.Name == "AntiAdictionResources.bundle";
                                 });

            return allBundles.ToArray();
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
                                     return framework.Name == "FTapTapOpenAPI.framework";
                                 });

            return allXCFramework.ToArray();
        }

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var taptapURLScheme = urlSchemeArray.AddDict();
            taptapURLScheme.SetString("CFBundleTypeRole", "Editor");
            taptapURLScheme.SetString("CFBundleURLName", "TapTap SDK");
            var taptapSchemes = taptapURLScheme.CreateArray("CFBundleURLSchemes");
            taptapSchemes.AddString($"tt{Config.TapTap.clientID}");

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("tapiosdk");
            queriesSchemes.AddString("tapsdk");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_TAPTAP_CLIENTID", Config.TapTap.clientID);
            sofunnyDict.SetString("FUNNY_TAPTAP_CLIENTTOKEN", Config.TapTap.clientToken);
            sofunnyDict.SetString("FUNNY_TAPTAP_SERVERURL", Config.TapTap.serverURL);
            sofunnyDict.SetBoolean("FUNNY_TAPTAP_BONFIRE", Config.TapTap.isBonfire);
        }

    }
}

#endif