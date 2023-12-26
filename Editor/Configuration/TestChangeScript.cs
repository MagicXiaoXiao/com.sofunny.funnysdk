using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Editor
{
    public class TestChangeScript : Configuration.Standalone
    {
        [MenuItem("SoFunnySDK/TestNewConfig", priority = 100)]
        public static void OnTestEditorAction()
        {

            //Configuration.Standalone standalone = Configuration.GetStandaloneConfiguration();
            //InitConfig initConfig = standalone.SetupInit();

            //string jsonData = JsonConvert.SerializeObject(initConfig);
            //Debug.Log("jsonData = " + jsonData);

            //InitConfig myConfig = JsonConvert.DeserializeObject<InitConfig>(jsonData);

            //Debug.Log(myConfig);


            //string content = "这是一段测试用例";
            //string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
            //string path = "/Users/cxb/Desktop/Doc/xiaobo.xmfunny";

            //File.WriteAllText(path, base64, Encoding.UTF8);
            //// 设置文件为只读
            //File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.ReadOnly);
        }

        public override InitConfig SetupInit()
        {
            return InitConfig.Create("1000000001", FunnyEnv.Overseas);
        }

        //public override Apple SetupApple()
        //{
        //    return Apple.Create(true);
        //}

        //public override QQ SetupQQ()
        //{
        //    return QQ.Create("102003211");
        //}

        //public override WeChat SetupWeChat()
        //{
        //    return WeChat.Create("wxca14c9032894873d", "https://auth.xmfunny.com/");
        //}

    }
}

