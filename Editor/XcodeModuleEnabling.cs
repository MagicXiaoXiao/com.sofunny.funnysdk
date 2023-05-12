#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;
using System.Linq;
using System;

using Debug = UnityEngine.Debug;

namespace SoFunny.FunnySDK.Editor {

    public partial class XcodeBuildConfigUpdating {
        //[MenuItem("SoFunnySDK/Test")]
        //public static void TestMethod2()
        //{
        //    // 预留测试脚本作用
        //}

        //[PostProcessBuild(689)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            if (target != BuildTarget.iOS) {
                return;
            }

            FunnyConfig.Instance.SyncToConfig();

            var proj = Start(pathToBuiltProject);

            string pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            proj.WriteToFile(pbxProjectPath);
        }
        
    }

    partial class XcodeBuildConfigUpdating {

        private static string FRAMEWORK_ORIGIN_PATH => Path.Combine(FunnyConfig.Instance._defaultPluginPath, "ExtPackage/iOS");

        private const string FRAMEWORK_TARGET_PATH = "Frameworks/com.sofunny.funnysdk/Plugins/iOS";
        private const string FUNNY_FRAMEWORK_EXTENSION = ".framework";

        internal static PBXProject Start(string pathToBuiltProject) {

            string pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(pbxProjectPath);

#if UNITY_2019_3_OR_NEWER
            string unityPackageTargetGUID = proj.GetUnityFrameworkTargetGuid();
            string projectTargetGUID = proj.GetUnityMainTargetGuid();
#else
            var projectTarget = proj.TargetGuidByName("Unity-iPhone");
#endif

            SetAppleSignInCer(proj, pbxProjectPath);

            SetXCFramework(proj, pathToBuiltProject);

            SetBuildProperty(proj);

            return proj;

        }

        /// <summary>
        /// 苹果登录证书相关配置
        /// </summary>
        private static void SetAppleSignInCer(PBXProject proj, string pbxProjectPath) {

            if (FunnyConfig.Instance.isMainland && !FunnyConfig.Instance.Apple.mainlandEnable) { return; }
            if (!FunnyConfig.Instance.isMainland && !FunnyConfig.Instance.Apple.overseaEnable) { return; }

            string projectTargetGUID = proj.GetUnityMainTargetGuid();

            var mainTargetName = "Unity-iPhone";

            proj.AddFrameworkToProject(projectTargetGUID, "AuthenticationServices.framework", true);
            var entitlementsPath = $"{mainTargetName}/{mainTargetName}.entitlements";

            var manager = new ProjectCapabilityManager(pbxProjectPath, entitlementsPath, targetGuid: projectTargetGUID);
            manager.AddSignInWithApple();
            manager.WriteToFile();
            proj.AddFile(entitlementsPath, $"{mainTargetName}.entitlements");
            proj.AddCapability(projectTargetGUID, PBXCapabilityType.SignInWithApple, entitlementsPath, false);
        }

        private static void SetXCFramework(PBXProject proj, string pathToBuiltProject) {

            string unityPackageTargetGUID = proj.GetUnityFrameworkTargetGuid();
            string projectTargetGUID = proj.GetUnityMainTargetGuid();


            var allXCFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) => {
                                     return Path.GetExtension(dirPath) == FUNNY_FRAMEWORK_EXTENSION;
                                 })
                                 .Select((dirPath) => {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) => {
                                     if (framework.Name == "FAppleServiceAPI.framework") {

                                         if (FunnyConfig.Instance.isMainland && FunnyConfig.Instance.Apple.mainlandEnable) {
                                             return true;
                                         }
                                         else if (!FunnyConfig.Instance.isMainland && FunnyConfig.Instance.Apple.overseaEnable) {
                                             return true;
                                         }
                                         else {
                                             return false;
                                         }

                                     }
                                     else if (FunnyConfig.Instance.Facebook.Enable && framework.Name == "FFacebookOpenAPI.framework") {
                                         return true;
                                     }
                                     else if (FunnyConfig.Instance.Twitter.Enable && framework.Name == "FTwitterOpenAPI.framework") {
                                         return true;
                                     }
                                     else if (FunnyConfig.Instance.WeChat.Enable && framework.Name == "FWeChatOpenAPI.framework") {
                                         return true;
                                     }
                                     else if (FunnyConfig.Instance.QQ.Enable && framework.Name == "FTencentOpenAPI.framework") {
                                         return true;
                                     }
                                     return false;
                                 });

            // Add SoFunny Frameworks
            foreach (var framework in allXCFramework) {

                var groupFolder = Path.Combine(FRAMEWORK_TARGET_PATH, framework.Name);

                FunnyUtils.Copy(framework.FullName, Path.Combine(pathToBuiltProject, groupFolder));

                var folderGuid = proj.AddFile(groupFolder, groupFolder);
                var mainLinkPhaseGuid = proj.GetFrameworksBuildPhaseByTarget(projectTargetGUID);

                proj.AddFileToBuildSection(projectTargetGUID, mainLinkPhaseGuid, folderGuid);
                proj.AddFileToBuild(unityPackageTargetGUID, folderGuid);
                proj.AddFileToEmbedFrameworks(projectTargetGUID, folderGuid);
            }
        }

        private static void SetBuildProperty(PBXProject proj) {

            string unityPackageTargetGUID = proj.GetUnityFrameworkTargetGuid();
            string projectTargetGUID = proj.GetUnityMainTargetGuid();

            proj.SetBuildProperty(unityPackageTargetGUID, "CLANG_ENABLE_MODULES", "YES");
            proj.SetBuildProperty(projectTargetGUID, "CLANG_ENABLE_MODULES", "YES");

            proj.SetBuildProperty(unityPackageTargetGUID, "CLANG_MODULES_AUTOLINK", "YES");
            proj.SetBuildProperty(projectTargetGUID, "CLANG_MODULES_AUTOLINK", "YES");

            proj.SetBuildProperty(unityPackageTargetGUID, "SWIFT_VERSION", "5.0");
            proj.SetBuildProperty(projectTargetGUID, "SWIFT_VERSION", "5.0");

            proj.SetBuildProperty(unityPackageTargetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.SetBuildProperty(projectTargetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            proj.AddBuildProperty(projectTargetGUID, "FRAMEWORK_SEARCH_PATHS", $"$(PROJECT_DIR)/{FRAMEWORK_TARGET_PATH}");
        }

    }

}
#endif