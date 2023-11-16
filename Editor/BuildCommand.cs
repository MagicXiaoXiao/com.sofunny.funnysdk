using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SoFunny.FunnySDK.Editor {
    public class BuildCommand {
        public static void Build() {
            string[] args = Environment.GetCommandLineArgs();
            string platform = "linux";
            bool isDev = true;
            string output = "";

            for (int i = 0; i < args.Length; i++) {
                if (args[i] == "-platform") {
                    platform = args[i + 1];
                }

                if (args[i] == "-dev") {
                    isDev = args[i + 1] == "1";
                }

                if (args[i] == "-output") {
                    output = args[i + 1];
                }
            }

            Debug.Log($"开始构建 App: {platform}, {isDev}, {output}");

            try {
                Build(platform, isDev, output);
            } catch (Exception e) {
                throw e;
            }
        }

        private static void Build(string platform, bool isDev, string output) {
            List<string> scenes = new List<string>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                if (!scene.enabled) {
                    continue;
                }

                scenes.Add(scene.path);
            }

            if (string.IsNullOrEmpty(output)) {
                output = $"{Application.dataPath}/../Builds/{platform}";
            }

            if (Directory.Exists(output)) {
                Directory.Delete(output, true);
            }

            Directory.CreateDirectory(output);

            BuildTarget buildTarget = GetBuildTarget(platform);
            string locationPathName = $"{output}/{GetBuildLocationName(buildTarget)}";

            BuildPlayerOptions options = new BuildPlayerOptions {
                scenes = scenes.ToArray(),
                locationPathName = locationPathName,
                target = buildTarget,
                options = isDev ? BuildOptions.Development : BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded) {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            } else if (summary.result == BuildResult.Failed) {
                Debug.Log("Build failed");
            }
        }

        private static BuildTarget GetBuildTarget(string platform) {
            switch (platform) {
                case "windows":
                    return BuildTarget.StandaloneWindows;
                case "ios":
                    return BuildTarget.iOS;
                case "linux":
                    return BuildTarget.StandaloneLinux64;
                // ...
                default:
                    return BuildTarget.Android;
            }
        }

        private static string GetBuildLocationName(BuildTarget target) {
            string targetName = $"{PlayerSettings.productName}-v{PlayerSettings.bundleVersion}";

            switch (target) {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return $"{targetName}.exe";
                case BuildTarget.Android:
                    return $"{targetName}{(EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk")}";
                // ...
                default:
                    return targetName;
            }
        }
    }
}