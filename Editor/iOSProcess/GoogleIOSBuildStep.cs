#if UNITY_IOS
using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class GoogleIOSBuildStep : FunnyXcodeBuildStep
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
                    return Config.Google.Enable;
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
                                     return framework.Name == "FGoogleSignIn.framework";
                                 });

            return allXCFramework.ToArray();
        }

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var facebookURLScheme = urlSchemeArray.AddDict();
            facebookURLScheme.SetString("CFBundleTypeRole", "Editor");
            facebookURLScheme.SetString("CFBundleURLName", "GOOGLE SDK");
            var fbSchemes = facebookURLScheme.CreateArray("CFBundleURLSchemes");
            fbSchemes.AddString($"{Config.Google.iOSURLScheme}");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;
            sofunnyDict.SetString("FUNNY_GOOGLE_CLIENT_ID", $"{Config.Google.iOSClientID}");
        }

    }
}

#endif