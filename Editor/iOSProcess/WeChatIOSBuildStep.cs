#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class WeChatIOSBuildStep : FunnyXcodeBuildStep
    {
        public override bool IsEnabled
        {
            get
            {
                if (FunnyConfig.Instance.isMainland)
                {
                    return FunnyConfig.Instance.WeChat.Enable;
                }
                else
                {
                    return false;
                }
            }
        }

        public override DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var allXCFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) => {
                                     return Path.GetExtension(dirPath) == FUNNY_FRAMEWORK_EXTENSION;
                                 })
                                 .Select((dirPath) => {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) => {
                                     return framework.Name == "FWeChatOpenAPI.framework";
                                 });

            return allXCFramework.ToArray();
        }


        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var wechatURLScheme = urlSchemeArray.AddDict();
            wechatURLScheme.SetString("CFBundleTypeRole", "Editor");
            wechatURLScheme.SetString("CFBundleURLName", "WECHAT SDK");
            var wechatSchemes = wechatURLScheme.CreateArray("CFBundleURLSchemes");
            wechatSchemes.AddString(FunnyConfig.Instance.WeChat.appID);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString("weixinULAPI");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_WECHAT_APPID", FunnyConfig.Instance.WeChat.appID);
            sofunnyDict.SetString("FUNNY_WECHAT_UNIVERSALLINK", FunnyConfig.Instance.WeChat.universalLink);
        }

    }
}

#endif