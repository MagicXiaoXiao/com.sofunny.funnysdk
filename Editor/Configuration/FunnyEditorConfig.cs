using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoFunny.FunnySDK.Internal;
using UnityEditor;

namespace SoFunny.FunnySDK.Editor
{

    public static class FunnyEditorConfig
    {
        private const string ConfigFilePath = "Packages/com.sofunny.funnysdk/Resources/FunnySDK/SDKConfig/SDKConfig.asset";

        private static SDKConfig config;

        public static SDKConfig Get()
        {
            if (!File.Exists(ConfigFilePath))
            {
                Debug.LogError("FunnySDK 插件包缺少配置信息，请重新安装此插件");
                return null;
            }

            if (config == null)
            {
                config = AssetDatabase.LoadAssetAtPath<SDKConfig>(ConfigFilePath);
            }
            return config;
        }

        // 同步配置文件数据
        public static void SyncData()
        {
            EditorUtility.SetDirty(Get());
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // 检查是否存在配置文件，不存在则创建
        private static void CheckConfigFile()
        {
            if (!File.Exists(ConfigFilePath))
            {
                var config = ScriptableObject.CreateInstance<SDKConfig>();
                AssetDatabase.CreateAsset(config, ConfigFilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

    }

}

