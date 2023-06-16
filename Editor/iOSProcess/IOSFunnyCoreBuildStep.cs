
#if UNITY_IOS

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace SoFunny.FunnySDK.Editor
{

    public class IOSFunnyCoreBuildStep : FunnyXcodeBuildStep
    {
        private FunnySDK.FunnySDKConfig Config => FunnyEditorConfig.GetConfig();

        public override bool IsEnabled => true;

        public override void OnProcessInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument infoPlist)
        {
            // setup schemes handler prefix
            PlistElementDict rootDict = infoPlist.root;
            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("funnyauth2");

            // setup schemes url prefix
            PlistElementArray array = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var funnyURLScheme = array.AddDict();
            funnyURLScheme.SetString("CFBundleTypeRole", "Editor");
            funnyURLScheme.SetString("CFBundleURLName", "FUNNY SDK");
            var schemes = funnyURLScheme.CreateArray("CFBundleURLSchemes");
            schemes.AddString("funny3rdp." + rootDict["CFBundleIdentifier"].AsString());

        }


        public override void OnProcessSoFunnyInfoPlist(BuildTarget buildTarget, string pathToBuiltTarget, PlistDocument sofunnyPlist)
        {
            PlistElementDict sofunnyDict = sofunnyPlist.root;
            bool isMainland = Config.IsMainland;

            sofunnyDict.SetBoolean("MAINLAND", isMainland);
            sofunnyDict.SetString("FUNNY_APP_ID", Config.AppID);

        }

    }

}

#endif