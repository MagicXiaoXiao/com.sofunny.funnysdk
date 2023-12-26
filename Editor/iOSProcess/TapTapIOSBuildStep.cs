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
        private TapTap tapTapConfig;
        private bool _enable = false;

        public override bool Enabled => _enable;

        internal override void OnInitConfig(Configuration.iOS config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (!editorConfig.IsMainland) return;

                tapTapConfig = TapTap.Create(editorConfig.TapTap.clientID, editorConfig.TapTap.clientToken, editorConfig.TapTap.serverURL);
                _enable = tapTapConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                tapTapConfig = config.SetupTapTap();

                if (initConfig.Env == FunnyEnv.Mainland)
                {
                    _enable = tapTapConfig.Enable;
                }
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
            taptapSchemes.AddString($"tt{tapTapConfig.ClientID}");

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("tapiosdk");
            queriesSchemes.AddString("tapsdk");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_TAPTAP_CLIENTID", tapTapConfig.ClientID);
            sofunnyDict.SetString("FUNNY_TAPTAP_CLIENTTOKEN", tapTapConfig.ClientToken);
            sofunnyDict.SetString("FUNNY_TAPTAP_SERVERURL", tapTapConfig.ServerURL);
            sofunnyDict.SetBoolean("FUNNY_TAPTAP_BONFIRE", tapTapConfig.EnableTestVersion);
        }

    }
}

#endif