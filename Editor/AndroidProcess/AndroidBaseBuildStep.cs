
#if UNITY_ANDROID

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using System.Xml;
using System.IO;

namespace SoFunny.FunnySDK.Editor {

    public class AndroidBaseBuildStep
    {
        // 是否激活脚本
        public virtual bool IsEnabled => false;

        public virtual string DisplayName => GetType().Name;
        public virtual string NamespaceURI => "http://schemas.android.com/apk/res/android";
        public virtual string AAR_ORIGIN_PATH => "Packages/com.sofunny.funnysdk/ExtPackage/Android";
        public virtual bool IsDebug => EditorUserBuildSettings.development || EditorUserBuildSettings.androidBuildType == AndroidBuildType.Debug;

        public static AndroidBaseBuildStep[] ProjectBuildStepObjects()
        {
            var steps = new List<AndroidBaseBuildStep>();
            var funnyBuildStepTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                      from type in assembly.GetTypes()
                                      where typeof(AndroidBaseBuildStep).IsAssignableFrom(type) && type != typeof(AndroidBaseBuildStep)
                                      select type;

            foreach (var item in funnyBuildStepTypes)
            {
                var step = (AndroidBaseBuildStep)Activator.CreateInstance(item);
                if (step.IsEnabled)
                {
                    steps.Add(step);
                }
            }

            return steps.ToArray();
        }

        /// <summary>
        /// 导出流程开始时前置操作流程
        /// </summary>
        /// <param name="unityLibraryPath"></param>
        public virtual void OnBeginPostProcess(string unityLibraryPath) { }

        /// <summary>
        /// 添加自定义 AAR 文件操作流程
        /// </summary>
        /// <param name="unityLibraryPath"></param>
        public virtual FileInfo[] OnProcessPrepareAARFile(string unityLibraryPath) { return Array.Empty<FileInfo>(); }

        /// <summary>
        /// 编辑 Launcher 目录下的 build.gradle 文件操作流程
        /// </summary>
        /// <param name="gradle"></param>
        public virtual void OnProcessLauncherGradle(GradleConfig gradle) { }

        /// <summary>
        /// 编辑项目 gradle.properties 文件操作流程
        /// </summary>
        /// <param name="properties"></param>
        public virtual void OnProcessGradleProperties(Dictionary<string, string> properties) { }

        /// <summary>
        /// 编辑 unityLibrary 目录下的 build.gradle 文件操作流程
        /// </summary>
        /// <param name="gradle"></param>
        public virtual void OnProcessUnityLibraryGradle(GradleConfig gradle) { }

        /// <summary>
        /// 编辑 unityLibrary 目录下的 strings.xml 文件
        /// </summary>
        /// <param name="stringsXML"></param>
        public virtual void OnProcessUnityLibraryStrings(XmlDocument stringsXML) { }

        /// <summary>
        /// 编辑 unityLibrary 目录下的 AndroidManifest.xml 文件流程
        /// </summary>
        /// <param name="manifestXML"></param>
        public virtual void OnProcessUnityLibraryManifest(XmlDocument manifestXML) { }

        /// <summary>
        /// 流程完成后执行
        /// </summary>
        /// <param name="unityLibraryPath"></param>
        public virtual void OnFinalizePostProcess(string unityLibraryPath) { }
    }

}

#endif