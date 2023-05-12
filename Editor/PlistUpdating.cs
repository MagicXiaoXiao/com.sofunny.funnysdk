#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;


namespace SoFunny.FunnySDK.Editor {

    public class PlistUpdating {


        [PostProcessBuild(688)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            if (target != BuildTarget.iOS) {
                return;
            }
            string pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(pbxProjectPath);

            var mainTarget = proj.GetUnityMainTargetGuid();

            string sofunnyPlistPath = pathToBuiltProject + "/SoFunny-Info.plist";
            string plistPath = pathToBuiltProject + "/Info.plist";

            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;

            PlistDocument sofunnyPlist = new PlistDocument();
            sofunnyPlist.Create();
            PlistElementDict sofunnyDict = sofunnyPlist.root;

            SetupPlist(rootDict, sofunnyDict);

            plist.WriteToFile(plistPath);
            sofunnyPlist.WriteToFile(sofunnyPlistPath);

            var fileGUID = proj.AddFile(sofunnyPlistPath, "SoFunny-Info.plist");
            var sectionGUID = proj.GetResourcesBuildPhaseByTarget(mainTarget);
            proj.AddFileToBuildSection(mainTarget, sectionGUID, fileGUID);
            proj.WriteToFile(pbxProjectPath);
        }

        internal static void SetupPlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {
            bool isMainland = FunnyConfig.Instance.isMainland;
            SetupCorePlist(rootDict, sofunnyDict);
            sofunnyDict.SetBoolean("MAINLAND", isMainland);
            sofunnyDict.SetString("FUNNY_APP_ID", FunnyConfig.Instance.appID);

            if (isMainland) {
                SetupWeChatPlist(rootDict, sofunnyDict);
                SetupQQPlist(rootDict, sofunnyDict);
            }
            else {
                SetupFacebookPlist(rootDict, sofunnyDict);
                SetupTwitterPlist(rootDict, sofunnyDict);
            }

        }

        static private void SetupCorePlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {

            // setup schemes handler prefix
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

        static private void SetupFacebookPlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {

            if (!FunnyConfig.Instance.Facebook.Enable) { return; }

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var facebookURLScheme = urlSchemeArray.AddDict();
            facebookURLScheme.SetString("CFBundleTypeRole", "Editor");
            facebookURLScheme.SetString("CFBundleURLName", "FACEBOOK SDK");
            var fbSchemes = facebookURLScheme.CreateArray("CFBundleURLSchemes");
            fbSchemes.AddString($"fb{FunnyConfig.Instance.Facebook.appID}");

            sofunnyDict.SetString("FUNNY_FACEBOOK_APPID", FunnyConfig.Instance.Facebook.appID);
            sofunnyDict.SetString("FUNNY_FACEBOOK_CLIENTTOKEN", FunnyConfig.Instance.Facebook.clientToken);
            sofunnyDict.SetBoolean("FUNNY_FACEBOOK_TRACK", FunnyConfig.Instance.Facebook.trackEnable);

            rootDict.SetBoolean("FacebookAutoLogAppEventsEnabled", false);
            rootDict.SetBoolean("FacebookAdvertiserIDCollectionEnabled", false);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("fbapi");
            queriesSchemes.AddString("fbauth");
            queriesSchemes.AddString("fbauth2");
            queriesSchemes.AddString("fbshareextension");
            queriesSchemes.AddString("fb-messenger-share-api");
        }

        static private void SetupTwitterPlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {

            if (!FunnyConfig.Instance.Twitter.Enable) { return; }

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var twitterURLScheme = urlSchemeArray.AddDict();
            twitterURLScheme.SetString("CFBundleTypeRole", "Editor");
            twitterURLScheme.SetString("CFBundleURLName", "TWITTER SDK");
            var twiSchemes = twitterURLScheme.CreateArray("CFBundleURLSchemes");
            twiSchemes.AddString($"twitterkit-{FunnyConfig.Instance.Twitter.consumerKey}");

            sofunnyDict.SetString("FUNNY_TWITTER_CKEY", FunnyConfig.Instance.Twitter.consumerKey);
            sofunnyDict.SetString("FUNNY_TWITTER_CSECRET", FunnyConfig.Instance.Twitter.consumerSecret);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("twitter");
            queriesSchemes.AddString("twitterauth");
        }

        static private void SetupWeChatPlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {

            if (!FunnyConfig.Instance.WeChat.Enable) { return; }

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var wechatURLScheme = urlSchemeArray.AddDict();
            wechatURLScheme.SetString("CFBundleTypeRole", "Editor");
            wechatURLScheme.SetString("CFBundleURLName", "WECHAT SDK");
            var wechatSchemes = wechatURLScheme.CreateArray("CFBundleURLSchemes");
            wechatSchemes.AddString(FunnyConfig.Instance.WeChat.appID);

            sofunnyDict.SetString("FUNNY_WECHAT_APPID", FunnyConfig.Instance.WeChat.appID);
            sofunnyDict.SetString("FUNNY_WECHAT_UNIVERSALLINK", FunnyConfig.Instance.WeChat.universalLink);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("weixin");
            queriesSchemes.AddString("weixinULAPI");
        }

        static private void SetupQQPlist(PlistElementDict rootDict, PlistElementDict sofunnyDict) {

            if (!FunnyConfig.Instance.QQ.Enable) { return; }

            PlistElementArray urlSchemeArray = GetOrCreateArray(rootDict, "CFBundleURLTypes");
            var qqURLScheme = urlSchemeArray.AddDict();
            qqURLScheme.SetString("CFBundleTypeRole", "Editor");
            qqURLScheme.SetString("CFBundleURLName", "QQ SDK");
            var qqSchemes = qqURLScheme.CreateArray("CFBundleURLSchemes");
            qqSchemes.AddString($"tencent{FunnyConfig.Instance.QQ.appID}");

            sofunnyDict.SetString("FUNNY_TENCENT_APPID", FunnyConfig.Instance.QQ.appID);
            sofunnyDict.SetString("FUNNY_TENCENT_UNIVERSALLINK", FunnyConfig.Instance.QQ.universalLink);

            PlistElementArray queriesSchemes = GetOrCreateArray(rootDict, "LSApplicationQueriesSchemes");
            queriesSchemes.AddString("mqq");
            queriesSchemes.AddString("mqqapi");
            queriesSchemes.AddString("mqqopensdkapiV2");
            queriesSchemes.AddString("mqqopensdknopasteboard");
        }

        static PlistElementArray GetOrCreateArray(PlistElementDict dict, string key) {
            PlistElement array = dict[key];
            if (array != null) {
                return array.AsArray();
            }
            else {
                return dict.CreateArray(key);
            }
        }

    }
}


#endif