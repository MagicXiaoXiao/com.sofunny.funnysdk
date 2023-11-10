#if UNITY_IOS

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;


namespace SoFunny.FunnySDK.Editor
{

    public class XcodeExportProcess
    {
        private const string FRAMEWORK_TARGET_PATH = "Frameworks/com.sofunny.funnysdk/Plugins/iOS";

        [PostProcessBuild(700)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS) { return; }

            FunnyEditorConfig.SyncData();

            var buildStepList = FunnyXcodeBuildStep.ProjectBuildStepObjects();

            #region 开始导出前置流程

            string pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(pbxProjectPath);

            foreach (var step in buildStepList)
            {
                step.OnBeginPostProcess(target, pathToBuiltProject);
            }

            #endregion

            #region 配置项目 Info.plist 文件流程

            string plistPath = pathToBuiltProject + "/Info.plist";
            var infoPlist = new PlistDocument();

            infoPlist.ReadFromString(File.ReadAllText(plistPath));

            foreach (var step in buildStepList)
            {
                step.OnProcessInfoPlist(target, pathToBuiltProject, infoPlist);
            }

            infoPlist.WriteToFile(plistPath);

            #endregion

            #region 配置项目 SoFunny-Info.plist 文件流程

            string sofunnyPlistPath = pathToBuiltProject + "/SoFunny-Info.plist";
            string mainTargetGUID = proj.GetUnityMainTargetGuid();
            string unityPackageTargetGUID = proj.GetUnityFrameworkTargetGuid();

            PlistDocument sofunnyPlist = new PlistDocument();
            sofunnyPlist.Create();

            foreach (var step in buildStepList)
            {
                step.OnProcessSoFunnyInfoPlist(target, pathToBuiltProject, sofunnyPlist);
            }

            sofunnyPlist.WriteToFile(sofunnyPlistPath);

            var fileGUID = proj.AddFile(sofunnyPlistPath, "SoFunny-Info.plist");
            var sectionGUID = proj.GetResourcesBuildPhaseByTarget(mainTargetGUID);
            proj.AddFileToBuildSection(mainTargetGUID, sectionGUID, fileGUID);

            #endregion

            #region 配置对应 Bundle 资源文件流程
            var resourcesBuildPhase = proj.GetResourcesBuildPhaseByTarget(mainTargetGUID);

            foreach (var step in buildStepList)
            {
                var allBundles = step.OnProcessBundles(target, pathToBuiltProject, proj);
                foreach (var bundle in allBundles)
                {
                    var groupFolder = Path.Combine(FRAMEWORK_TARGET_PATH, bundle.Name);

                    FunnyUtils.Copy(bundle.FullName, Path.Combine(pathToBuiltProject, groupFolder));

                    var resourcesFilesGuid = proj.AddFolderReference(groupFolder, groupFolder, PBXSourceTree.Source);

                    proj.AddFileToBuildSection(mainTargetGUID, resourcesBuildPhase, resourcesFilesGuid);
                }
            }
            #endregion

            #region 配置对应 Framework 资源文件流程

            foreach (var step in buildStepList)
            {
                var allFrameworks = step.OnProcessFrameworks(target, pathToBuiltProject, proj);
                var mainLinkPhaseGuid = proj.GetFrameworksBuildPhaseByTarget(mainTargetGUID);

                // Add SoFunny Frameworks
                foreach (var framework in allFrameworks)
                {

                    var groupFolder = Path.Combine(FRAMEWORK_TARGET_PATH, framework.Name);

                    FunnyUtils.Copy(framework.FullName, Path.Combine(pathToBuiltProject, groupFolder));

                    var folderGuid = proj.AddFile(groupFolder, groupFolder);

                    proj.AddFileToBuildSection(mainTargetGUID, mainLinkPhaseGuid, folderGuid);
                    proj.AddFileToBuild(unityPackageTargetGUID, folderGuid);
                    proj.AddFileToEmbedFrameworks(mainTargetGUID, folderGuid);
                }
            }

            #endregion

            #region 设置 Xcode BuildSettings 配置参数流程

            proj.AddBuildProperty(mainTargetGUID, "FRAMEWORK_SEARCH_PATHS", $"$(PROJECT_DIR)/{FRAMEWORK_TARGET_PATH}");

            proj.SetBuildProperty(unityPackageTargetGUID, "CLANG_ENABLE_MODULES", "YES");
            proj.SetBuildProperty(mainTargetGUID, "CLANG_ENABLE_MODULES", "YES");

            proj.SetBuildProperty(unityPackageTargetGUID, "CLANG_MODULES_AUTOLINK", "YES");
            proj.SetBuildProperty(mainTargetGUID, "CLANG_MODULES_AUTOLINK", "YES");

            proj.SetBuildProperty(unityPackageTargetGUID, "SWIFT_VERSION", "5.0");
            proj.SetBuildProperty(mainTargetGUID, "SWIFT_VERSION", "5.0");

            //proj.SetBuildProperty(unityPackageTargetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            //proj.SetBuildProperty(mainTargetGUID, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

            proj.SetBuildProperty(unityPackageTargetGUID, "ENABLE_BITCODE", "NO");
            proj.SetBuildProperty(mainTargetGUID, "ENABLE_BITCODE", "NO");

            #endregion

            #region 收尾流程

            if (proj != null)
            {
                proj.WriteToFile(pbxProjectPath);
            }

            foreach (var buildStep in buildStepList)
            {
                buildStep.OnFinalizePostProcess(target, pathToBuiltProject);
            }

            #endregion
        }
    }

}

#endif