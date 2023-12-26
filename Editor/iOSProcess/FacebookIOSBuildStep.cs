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
        private Facebook facebookConfig;
        private bool _enableValue = false;

        public override bool Enabled => _enableValue;

        internal override void OnInitConfig(Configuration.iOS config)
        {
            if (config is null) // 执行旧配置逻辑
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();

                if (editorConfig.IsMainland) return;

                facebookConfig = Facebook.Create(editorConfig.Facebook.appID, editorConfig.Facebook.clientToken);
                facebookConfig.SetAdvertiserTracking(editorConfig.Facebook.trackEnable);

                _enableValue = editorConfig.Facebook.Enable;
            }
            else // 执行新配置逻辑
            {
                InitConfig initConfig = config.SetupInit();
                facebookConfig = config.SetupFacebook();

                if (initConfig.Env == FunnyEnv.Overseas)
                {
                    _enableValue = facebookConfig.Enable;
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
            fbSchemes.AddString($"fb{facebookConfig.AppID}");

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

            sofunnyDict.SetString("FUNNY_FACEBOOK_APPID", facebookConfig.AppID);
            sofunnyDict.SetString("FUNNY_FACEBOOK_CLIENTTOKEN", facebookConfig.ClientToken);
            sofunnyDict.SetBoolean("FUNNY_FACEBOOK_TRACK", facebookConfig.EnableAdvertiserTrack);
        }

    }
}

#endif