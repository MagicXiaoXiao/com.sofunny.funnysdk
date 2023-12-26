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
        private WeChat wechatConfig;
        private bool _enable = false;

        public override bool Enabled => _enable;

        internal override void OnInitConfig(Configuration.iOS config)
        {
            if (config is null)
            {
                FunnySDKConfig editorConfig = FunnyEditorConfig.GetConfig();
                if (!editorConfig.IsMainland) return;

                wechatConfig = WeChat.Create(editorConfig.WeChat.appID, editorConfig.WeChat.universalLink);
                _enable = wechatConfig.Enable;
            }
            else
            {
                InitConfig initConfig = config.SetupInit();
                wechatConfig = config.SetupWeChat();

                if (initConfig.Env == FunnyEnv.Mainland)
                {
                    _enable = wechatConfig.Enable;
                }
            }
        }

        public override DirectoryInfo[] OnProcessFrameworks(BuildTarget buildTarget, string pathToBuiltTarget, PBXProject pBXProject)
        {
            var allXCFramework = Directory.GetDirectories(FRAMEWORK_ORIGIN_PATH)
                                 .Where((dirPath) =>
                                 {
                                     return Path.GetExtension(dirPath) == FUNNY_FRAMEWORK_EXTENSION;
                                 })
                                 .Select((dirPath) =>
                                 {
                                     return new DirectoryInfo(dirPath);
                                 })
                                 .Where((framework) =>
                                 {
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
            wechatSchemes.AddString(wechatConfig.AppID);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString("weixinULAPI");
            queriesSchemes.AddString("weixinURLParamsAPI");

        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_WECHAT_APPID", wechatConfig.AppID);
            sofunnyDict.SetString("FUNNY_WECHAT_UNIVERSALLINK", wechatConfig.UniversalLinks);
        }

    }
}

#endif