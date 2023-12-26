using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Editor
{

    public static partial class Configuration
    {

#if UNITY_EDITOR

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnRuntimeEditorFunnySDK()
        {
#if UNITY_IOS
            iOS ios = GetIosConfiguration();
            InitConfig initConfig = ios?.SetupInit();
#endif

#if UNITY_ANDROID
            Android android = GetAndroidConfiguration();
            InitConfig initConfig = android?.SetupInit();
#endif

#if UNITY_STANDALONE
            Standalone standalone = GetStandaloneConfiguration();
            InitConfig initConfig = standalone?.SetupInit();
#endif
            if (initConfig is null) return;

            string jsonConfig = JsonConvert.SerializeObject(initConfig.GenerateNativeConfig());
            EditorPrefs.SetString("funnysdk.editor.app.config", jsonConfig);
        }

#endif

        internal static Configuration.iOS GetIosConfiguration()
        {
            Type configType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(Configuration.iOS).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .FirstOrDefault();

            if (configType is null) return null;

            return (Configuration.iOS)Activator.CreateInstance(configType, true);
        }

        internal static Configuration.Android GetAndroidConfiguration()
        {
            Type configType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(Configuration.Android).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .FirstOrDefault();

            if (configType is null) return null;

            return (Configuration.Android)Activator.CreateInstance(configType, true);
        }

        internal static Configuration.Standalone GetStandaloneConfiguration()
        {
            Type configType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(Configuration.Standalone).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .FirstOrDefault();

            if (configType is null) return null;

            return (Configuration.Standalone)Activator.CreateInstance(configType, true);
        }
    }
}

