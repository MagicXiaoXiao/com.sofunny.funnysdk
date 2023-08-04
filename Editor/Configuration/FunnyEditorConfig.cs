using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SoFunny.FunnySDK.Editor
{

    public static class FunnyEditorConfig
    {
        private const string ConfigFilePath = "Assets/Resources/FunnySDK/ServiceConfig.asset";
        private static FunnySDKConfig SDKConfig;

        public static FunnySDKConfig GetConfig()
        {
            if (SDKConfig is null)
            {
                SDKConfig = AssetDatabase.LoadAssetAtPath<FunnySDKConfig>(ConfigFilePath);

                if (SDKConfig is null)
                {
                    Debug.LogError("FunnySDK 插件包缺少配置信息，请先生成配置文件。");
                    return null;
                }
            }
            return SDKConfig;
        }

        // 同步配置文件数据
        public static void SyncData()
        {
            if (SDKConfig is null)
            {
                GetConfig();
            }

            EditorUtility.SetDirty(SDKConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // 检查是否存在配置文件
        public static bool CheckConfigFile()
        {
            FunnySDKConfig hasConfig = AssetDatabase.LoadAssetAtPath<FunnySDKConfig>(ConfigFilePath);

            return !(hasConfig is null); // is not null 语法 Unity 编译器暂不支持，故做此处理
        }

        public static void CreateConfigFile()
        {
            if (!Directory.Exists("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            if (!Directory.Exists("Assets/Resources/FunnySDK"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "FunnySDK");
            }

            FunnySDKConfig createConfig = ScriptableObject.CreateInstance<FunnySDKConfig>();
            AssetDatabase.CreateAsset(createConfig, ConfigFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DeleteConfigFile()
        {
            AssetDatabase.DeleteAsset(ConfigFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }

}

