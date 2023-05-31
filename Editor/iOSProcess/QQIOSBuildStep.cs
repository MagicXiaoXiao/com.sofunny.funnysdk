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
        private FunnySDK.Internal.SDKConfig Config => FunnyEditorConfig.Get();

        public override bool IsEnabled
        {
            get
            {
                return Config.IsMainland && Config.QQ.Enable;
            }
        }

        public override DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var allXCFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) => {
                                     return Path.GetExtension(dirPath) == FUNNY_FRAMEWORK_EXTENSION;
                                 })
                                 .Select((dirPath) => {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) => {
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
            qqSchemes.AddString($"tencent{Config.QQ.appID}");


            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("mqq");
            queriesSchemes.AddString("mqqapi");
            queriesSchemes.AddString("mqqopensdkapiV2");
            queriesSchemes.AddString("mqqopensdknopasteboard");

        }


        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_TENCENT_APPID", Config.QQ.appID);
            sofunnyDict.SetString("FUNNY_TENCENT_UNIVERSALLINK", Config.QQ.universalLink);
        }
    }
}

#endif