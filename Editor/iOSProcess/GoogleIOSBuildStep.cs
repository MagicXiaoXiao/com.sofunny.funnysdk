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
        private Google googleConfig;
        private bool _enable = false;

        public override bool Enabled => _enable;

        internal override void OnInitConfig(Configuration.iOS config)
        {
            if (config is null)
            {
                FunnySDKConfig Config = FunnyEditorConfig.GetConfig();

                if (Config.IsMainland) return;

                _enable = Config.Google.Enable;
                googleConfig = Google.Create(Config.Google.iOSClientID);
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                googleConfig = config.SetupGoogle();

                if (initConfig.Env == FunnyEnv.Overseas)
                {
                    _enable = googleConfig.Enable;
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
            fbSchemes.AddString($"{googleConfig.ReversedClientID}");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;
            sofunnyDict.SetString("FUNNY_GOOGLE_CLIENT_ID", $"{googleConfig.ClientID}");
        }

    }
}

#endif