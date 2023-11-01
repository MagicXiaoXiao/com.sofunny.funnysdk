#if UNITY_IOS

using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace SoFunny.FunnySDK.Editor
{
    public class AppleWithSignIOSBuildStep : FunnyXcodeBuildStep
    {

        public override bool IsEnabled
        {
            get
            {
                var config = FunnyEditorConfig.GetConfig();
                if (config.IsMainland)
                {
                    return config.Apple.mainlandEnable;
                }
                else
                {
                    return config.Apple.overseaEnable;
                }
            }
        }

        public override DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            string projectTargetGUID = pBXProject.GetUnityMainTargetGuid();

            var mainTargetName = "Unity-iPhone";

            pBXProject.AddFrameworkToProject(projectTargetGUID, "AuthenticationServices.framework", true);
            var entitlementsPath = $"{mainTargetName}/{mainTargetName}.entitlements";

            PlistDocument sofunnyPlist = new PlistDocument();
            sofunnyPlist.Create();
            sofunnyPlist.root.CreateArray("com.apple.developer.applesignin").AddString("Default");
            sofunnyPlist.WriteToFile($"{pathToBuiltTarget}/{entitlementsPath}");

            //var manager = new ProjectCapabilityManager(pathToBuiltTarget, entitlementsPath, targetGuid: projectTargetGUID);
            //manager.AddSignInWithApple();
            //manager.WriteToFile();

            pBXProject.AddFile(entitlementsPath, $"{mainTargetName}.entitlements");
            pBXProject.AddCapability(projectTargetGUID, PBXCapabilityType.SignInWithApple, entitlementsPath, true);

            var allFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
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
                                    return framework.Name == "FAppleServiceAPI.framework";
                                });

            return allFramework.ToArray();
        }

    }

}

#endif