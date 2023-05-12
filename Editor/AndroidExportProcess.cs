
#if UNITY_ANDROID
using UnityEngine;
using UnityEditor.Android;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace SoFunny.FunnySDK.Editor {

    public class AndroidExportProcess: IPostGenerateGradleAndroidProject
    {
        public int callbackOrder
        {
            get
            {
                return 700;
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            FunnyConfig.Instance.SyncToConfig();
            // Android 导出脚本执行
            var buildStepList = AndroidBaseBuildStep.ProjectBuildStepObjects();

            #region 前置开始操作流程

            foreach (var step in buildStepList)
            {
                step.OnBeginPostProcess(path);
            }

            #endregion

            #region 配置 gradle.properties 文件操作流程

            DirectoryInfo androidPath = new DirectoryInfo(path);
            // 获取 gradle.properties 文件
            var files = androidPath.Parent.GetFiles("gradle.properties");

            // 不存在则创建
            if (files.Length <= 0)
            {
                var propPath = Path.Combine(androidPath.Parent.FullName, "gradle.properties");
                var propFile = File.Create(propPath);
                propFile.Flush();
                propFile.Close();
            }

            // 存在则获取首个文件并转为 FileInfo 对象
            var file = files.First();

            // 创建临时文件对象
            var tempFile = new FileInfo(path + "/properties.temp");

            // 创建临时解析数据对象
            Dictionary<string, string> properties = new Dictionary<string, string>();

            // 解析文件内容并添加到解析对象
            using (StreamReader sr = file.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var props = line.Split('=');
                    if (props.Length >= 2)
                    {
                        properties.Add(props[0], props[1]);
                    }
                }
            }

            foreach (var step in buildStepList)
            {
                step.OnProcessGradleProperties(properties);
            }

            // 写入临时配置文件
            using (StreamWriter sw = tempFile.CreateText())
            {

                foreach (var item in properties)
                {
                    sw.WriteLine($"{item.Key}={item.Value}");
                }
            }

            // 删除源文件
            file.Delete();
            // 移动临时文件到目标位置
            tempFile.MoveTo(file.FullName);

            #endregion

            #region 配置 launcher 下的 build.gradle 文件操作流程

            DirectoryInfo unityLibraryPath = new DirectoryInfo(path);
            var launcherDir = unityLibraryPath.Parent.GetDirectories().Where(x => x.Name == "launcher").First();
            var gradleFile = launcherDir.GetFiles("build.gradle").First();

            var launcherGradle = new GradleConfig(gradleFile.FullName);

            foreach (var step in buildStepList)
            {
                step.OnProcessLauncherGradle(launcherGradle);
            }

            launcherGradle.Save();
            #endregion

            #region 配置 unityLibrary 下的 build.gradle 文件操作流程

            DirectoryInfo unityLibrary = new DirectoryInfo(path);
            var gradleFiles = unityLibrary.GetFiles("build.gradle");
            var unityLibraryGradleFile = gradleFiles.First();

            var unityLibraryGradle = new GradleConfig(unityLibraryGradleFile.FullName);

            foreach (var step in buildStepList)
            {
                step.OnProcessUnityLibraryGradle(unityLibraryGradle);
            }

            unityLibraryGradle.Save();

            #endregion

            #region 配置 unityLibrary 下的 strings.xml 文件操作流程

            var stringsPath = Path.Combine(path, "src/main/res/values/strings.xml");
            XmlDocument stringsDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = stringsDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = stringsDoc.DocumentElement;
            stringsDoc.InsertBefore(xmlDeclaration, root);

            XmlElement resources = stringsDoc.CreateElement(string.Empty, "resources", string.Empty);
            stringsDoc.AppendChild(resources);

            foreach (var step in buildStepList)
            {
                step.OnProcessUnityLibraryStrings(stringsDoc);
            }

            stringsDoc.Save(stringsPath);

            #endregion

            #region 配置 unityLibrary 下的 AndroidManifest.xml 文件操作流程

            var manifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");

            XmlDocument manifestDoc = new XmlDocument();
            manifestDoc.Load(manifestPath);

            foreach (var step in buildStepList)
            {
                step.OnProcessUnityLibraryManifest(manifestDoc);
            }

            manifestDoc.Save(manifestPath);

            #endregion

            #region 配置自定义 AAR 文件流程

            var libDir = Path.Combine(path, "libs");

            foreach (var step in buildStepList)
            {
                var allAARFiles = step.OnProcessPrepareAARFile(path);
                // Add SoFunny aars
                foreach (var aarFile in allAARFiles)
                {
                    Debug.Log("aar path: " + aarFile.FullName + " lib path: " + libDir);
                    FunnyUtils.Copy(aarFile.FullName, Path.Combine(libDir, aarFile.Name));
                }
            }

            #endregion

            #region 收尾操作流程
            foreach (var step in buildStepList)
            {
                step.OnFinalizePostProcess(path);
            }
            #endregion

        }
    }
}

#endif
