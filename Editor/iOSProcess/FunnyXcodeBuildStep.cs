#if UNITY_IOS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif

namespace SoFunny.FunnySDK.Editor {


    public class FunnyXcodeBuildStep
    {

        public virtual bool IsEnabled => false;

        public virtual string DisplayName => GetType().Name;

        public virtual string FRAMEWORK_ORIGIN_PATH => Path.Combine(FunnyConfig.Instance._defaultPluginPath, "ExtPackage/iOS");

        public virtual string FUNNY_FRAMEWORK_EXTENSION => ".framework";

        public static FunnyXcodeBuildStep[] ProjectBuildStepObjects()
        {
            var steps = new List<FunnyXcodeBuildStep>();
            var funnyBuildStepTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                      from type in assembly.GetTypes()
                                      where typeof(FunnyXcodeBuildStep).IsAssignableFrom(type) && type != typeof(FunnyXcodeBuildStep)
                                      select type;

            foreach (var item in funnyBuildStepTypes)
            {
                var step = (FunnyXcodeBuildStep)Activator.CreateInstance(item);
                if (step.IsEnabled)
                {
                    steps.Add(step);
                }
            }

            return steps.ToArray();
        }

        public PlistElementArray GetOrCreateArray(PlistElementDict dict, string key)
        {
            PlistElement array = dict[key];
            if (array != null)
            {
                return array.AsArray();
            }
            else
            {
                return dict.CreateArray(key);
            }
        }


        public virtual void OnBeginPostProcess(BuildTarget buildTarget, string pathToBuiltProject) { }

        public virtual void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist) { }

        public virtual void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist) { }

        //public virtual void OnProcessEntitlements(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument entitlements) { }

        public virtual DirectoryInfo[] OnProcessBundles(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject) { return Array.Empty<DirectoryInfo>(); }

        public virtual DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject) { return Array.Empty<DirectoryInfo>(); }

        public virtual void OnFinalizePostProcess(BuildTarget buildTarget, string pathToBuiltProject) { }

    }

}

#endif