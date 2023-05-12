﻿#if UNITY_IOS

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace SoFunny.FunnySDK.Editor
{
    public class TwitterIOSBuildStep : FunnyXcodeBuildStep
    {
        public override bool IsEnabled
        {
            get
            {
                if (FunnyConfig.Instance.isMainland)
                {
                    return false;
                }
                else
                {
                    return FunnyConfig.Instance.Twitter.Enable;
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
                                     return framework.Name == "FTwitterOpenAPI.framework";
                                 });

            return allXCFramework.ToArray();
        }

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            var rootDict = infoPlist.root;

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var twitterURLScheme = urlSchemeArray.AddDict();
            twitterURLScheme.SetString("CFBundleTypeRole", "Editor");
            twitterURLScheme.SetString("CFBundleURLName", "TWITTER SDK");
            var twiSchemes = twitterURLScheme.CreateArray("CFBundleURLSchemes");
            twiSchemes.AddString($"twitterkit-{FunnyConfig.Instance.Twitter.consumerKey}");

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("twitter");
            queriesSchemes.AddString("twitterauth");
        }

        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            var sofunnyDict = sofunnyPlist.root;

            sofunnyDict.SetString("FUNNY_TWITTER_CKEY", FunnyConfig.Instance.Twitter.consumerKey);
            sofunnyDict.SetString("FUNNY_TWITTER_CSECRET", FunnyConfig.Instance.Twitter.consumerSecret);
        }

    }
}

#endif