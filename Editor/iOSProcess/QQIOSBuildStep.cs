#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class QQIOSBuildStep : FunnyXcodeBuildStep
    {
        private QQ qqConfig;
        private bool _enable = false;

        public override bool Enabled => _enable;

        internal override void OnInitConfig(Configuration.iOS config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (!editorConfig.IsMainland) return;

                qqConfig = QQ.Create(editorConfig.QQ.appID, editorConfig.QQ.universalLink);
                _enable = editorConfig.IsMainland && qqConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                qqConfig = config.SetupQQ();

                if (initConfig.Env == FunnyEnv.Mainland)
                {
                    _enable = qqConfig.Enable;
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
                                     return framework.Name == "FTencentOpenAPI.framework";
                                 });

            return allXCFramework.ToArray();
        }

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var qqURLScheme = urlSchemeArray.AddDict();
            qqURLScheme.SetString("CFBundleTypeRole", "Editor");
            qqURLScheme.SetString("CFBundleURLName", "QQ SDK");
            var qqSchemes = qqURLScheme.CreateArray("CFBundleURLSchemes");
            qqSchemes.AddString($"tencent{qqConfig.AppID}");


            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("mqq");
            queriesSchemes.AddString("mqqapi");
            queriesSchemes.AddString("mqqopensdkapiV2");
            queriesSchemes.AddString("mqqopensdknopasteboard");

        }


        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_TENCENT_APPID", qqConfig.AppID);
            sofunnyDict.SetString("FUNNY_TENCENT_UNIVERSALLINK", qqConfig.UniversalLinks);
        }
    }
}

#endif