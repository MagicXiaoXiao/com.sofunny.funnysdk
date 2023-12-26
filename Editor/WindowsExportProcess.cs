#if UNITY_STANDALONE

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace SoFunny.FunnySDK.Editor
{
    public class WindowsExportProcess
    {
        private const string _configFileName = "funnysdk.core";

        [PostProcessBuild(700)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                string directoryPath = Path.GetDirectoryName(pathToBuiltProject);
                string productDataPath = Path.Combine(directoryPath, $"{Application.productName}_Data");

                Configuration.Standalone standalone = Configuration.GetStandaloneConfiguration();
                if (standalone is null)
                {
                    Debug.LogException(new NotImplementedException("未找到 Configuration.Standalone 子类配置项，无法进行 FunnySDK 构建流程"));
                    return;
                }

                string configJson = JsonConvert.SerializeObject(standalone.SetupInit().GenerateNativeConfig());
                string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(configJson));
                string filePath = Path.Combine(productDataPath, _configFileName);

                try
                {
                    // 将配置内容写入文件中
                    File.WriteAllText(filePath, base64Data, Encoding.UTF8);
                    // 设置文件为只读
                    File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.ReadOnly);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}

#endif