using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SoFunny.FunnySDK.Editor {

    public class SettingsWindow: EditorWindow {

        [MenuItem("SoFunnySDK/FunnySDK Settings")]
        public static void AttackWindowMethod() {
            SettingsWindow.Show();
            // EditorUtility.DisplayDialog("错误", "请选择已 FunnySDK 为名称的文件夹", "好的");
        }

        // Add menu named "My Window" to the Window menu
        internal static new void Show() {
            // Get existing open window or if none, make a new one:
            var window = EditorWindow.GetWindow<SettingsWindow>("FunnySDK Settings");
            window.ShowModal();
        }

        private void OnDestroy() {
            FunnyConfig.Instance.SyncToConfig();
        }

        float containerPadding = 12;
        Rect containerRect => new Rect(
            x: containerPadding,
            y: containerPadding,
            width: position.width - containerPadding * 2,
            height: position.height - containerPadding * 2
        );

        private GUIStyle appIDLabelStyle {
            get {
                var style = new GUIStyle(EditorStyles.boldLabel);
                return style;
            }
        }


        private GUIStyle funnyToggleStyle {
            get {
                var style = new GUIStyle() {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    clipping = TextClipping.Overflow,
                    alignment = TextAnchor.MiddleLeft
                };
                style.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                
                return style;
            }
        }

        private bool mainlandValue {
            get {
                return FunnyConfig.Instance.isMainland;
            }
            set {
                FunnyConfig.Instance.isMainland = value;
            }
        }

        private bool overseaValue {
            get {
                return !FunnyConfig.Instance.isMainland;
            }
            set {
                FunnyConfig.Instance.isMainland = !value;
            }
        }


        void BasicSettingsUI() {
            
            var boxStyle = new GUIStyle(EditorStyles.helpBox) {
                padding = new RectOffset(10,10,10,10)
            };

            EditorGUILayout.BeginVertical(boxStyle);

            EditorGUILayout.BeginHorizontal();
            overseaValue = EditorGUILayout.ToggleLeft("海外", overseaValue, funnyToggleStyle, GUILayout.Width(60));
            mainlandValue = EditorGUILayout.ToggleLeft("国内", mainlandValue, funnyToggleStyle, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AppID：", appIDLabelStyle, GUILayout.MaxWidth(50));
            FunnyConfig.Instance.appID = EditorGUILayout.TextField(FunnyConfig.Instance.appID, GUILayout.MaxWidth(200));
            EditorGUILayout.LabelField("*必填", GUILayout.MaxWidth(44));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (EditorGUI.actionKey) {
                EditorGUILayout.LabelField($"当前包名为：", GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField($"{PlayerSettings.applicationIdentifier}", GUILayout.ExpandWidth(false));
            }
            
            EditorGUILayout.EndVertical();

        }

        int mlSelectIndex = 0;
        int mlSelectChangeIndex = 0;

        void MainlandAreaUI() {
            // 国内配置显示逻辑
            var boxStyle = new GUIStyle(EditorStyles.helpBox) {
                padding = new RectOffset(10, 10, 10, 10)
            };
            EditorGUILayout.BeginVertical(boxStyle);

            mlSelectIndex = GUILayout.Toolbar(mlSelectIndex, new string[] { "TapTap", "QQ", "微信", "AppleSignIn" });

            EditorGUILayout.Separator();

            if (mlSelectIndex != mlSelectChangeIndex)
            {
                mlSelectChangeIndex = mlSelectIndex;
                EditorGUI.FocusTextInControl(null);
            }

            switch (mlSelectIndex) {
                case 0:

                    EditorGUILayout.LabelField("TapTap Client ID");
                    var taptapClientID = EditorGUILayout.TextField(FunnyConfig.Instance.TapTap.clientID);
                    EditorGUILayout.LabelField("TapTap Client Token");
                    var taptapClientToken = EditorGUILayout.TextField(FunnyConfig.Instance.TapTap.clientToken);
                    EditorGUILayout.LabelField("TapTap Server URL");
                    var taptapServerURL = EditorGUILayout.TextField(FunnyConfig.Instance.TapTap.serverURL);

                    EditorGUILayout.Separator();
                    var isBonfireValue = EditorGUILayout.ToggleLeft("开启篝火测试", FunnyConfig.Instance.TapTap.isBonfire, funnyToggleStyle);
                    FunnyConfig.Instance.TapTap = new TapTapConfig(taptapClientID, taptapClientToken, taptapServerURL, isBonfireValue);
                    break;
                case 1: // QQ
                    EditorGUILayout.LabelField("QQ App ID");
                    var qqAppID = EditorGUILayout.TextField(FunnyConfig.Instance.QQ.appID);
                    EditorGUILayout.LabelField("QQ Universal Link");
                    var qqlink = EditorGUILayout.TextField(FunnyConfig.Instance.QQ.universalLink);
                    FunnyConfig.Instance.QQ = new TencentQQConfig(qqAppID, qqlink);
                    break;
                case 2: // 微信
                    var wxTips = new GUIStyle();
                    wxTips.normal.textColor = EditorGUIUtility.isProSkin ? Color.yellow : Color.red;
                    EditorGUILayout.LabelField("如需接入微信，则 AppID 和 UniversalLink 参数必须填写", wxTips);
                    EditorGUILayout.LabelField("WeChat App ID");
                    var wxAppID = EditorGUILayout.TextField(FunnyConfig.Instance.WeChat.appID);
                    EditorGUILayout.LabelField("WeChat Universal Link");
                    var wxLink = EditorGUILayout.TextField(FunnyConfig.Instance.WeChat.universalLink);
                    FunnyConfig.Instance.WeChat = new WeChatConfig(wxAppID, wxLink);
                    break;
                case 3: // AppleSignIn
                    EditorGUILayout.BeginHorizontal();
                    var enable = EditorGUILayout.ToggleLeft("勾选则表示开启 SignIn With Apple", FunnyConfig.Instance.Apple.mainlandEnable);
                    FunnyConfig.Instance.Apple.mainlandEnable = enable;
                    EditorGUILayout.EndHorizontal();
                    break;
                default:
                    break;
            }

            EditorGUILayout.EndVertical();
        }

        int osSelectIndex = 0;
        int osSelectChangeIndex = 0;

        void OverseaAreaUI() {
            // 海外配置显示逻辑
            var boxStyle = new GUIStyle(EditorStyles.helpBox) {
                padding = new RectOffset(10, 10, 10, 10)
            };
            EditorGUILayout.BeginVertical(boxStyle);
            osSelectIndex = GUILayout.Toolbar(osSelectIndex, new string[] { "Google", "Facebook", "AppleSignIn", "Twitter" });

            EditorGUILayout.Separator();

            if (osSelectIndex != osSelectChangeIndex) {
                osSelectChangeIndex = osSelectIndex;
                EditorGUI.FocusTextInControl(null);
            }

            switch (osSelectIndex) {
                case 0: // Google
                    EditorGUILayout.LabelField("Google IDToken");
                    var idToken = EditorGUILayout.TextField(FunnyConfig.Instance.Google.idToken);
                    FunnyConfig.Instance.Google = new GoogleConfig(idToken);
                    break;
                case 1: // Facebook
                    EditorGUILayout.LabelField("Facebook App ID");
                    var appID = EditorGUILayout.TextField(FunnyConfig.Instance.Facebook.appID);
                    EditorGUILayout.LabelField("Facebook Client Token");
                    var clientToken = EditorGUILayout.TextField(FunnyConfig.Instance.Facebook.clientToken);
                    EditorGUILayout.Space();
                    var trackEnable = EditorGUILayout.ToggleLeft("启用 Facebook 自动数据收集", FunnyConfig.Instance.Facebook.trackEnable);
                    FunnyConfig.Instance.Facebook = new FacebookConfig(appID, clientToken, trackEnable);
                    break;
                case 2: // AppleSignIn
                    EditorGUILayout.BeginHorizontal();
                    var enable = EditorGUILayout.ToggleLeft("勾选则表示开启 SignIn With Apple", FunnyConfig.Instance.Apple.overseaEnable);
                    FunnyConfig.Instance.Apple.overseaEnable = enable;
                    EditorGUILayout.EndHorizontal();
                    break;
                case 3: // Twitter
                    EditorGUILayout.LabelField("Twitter Consumer Key");
                    var key = EditorGUILayout.TextField(FunnyConfig.Instance.Twitter.consumerKey);
                    EditorGUILayout.LabelField("Twitter Consumer Secret");
                    var secret = EditorGUILayout.TextField(FunnyConfig.Instance.Twitter.consumerSecret);
                    FunnyConfig.Instance.Twitter = new TwitterConfig(key, secret);
                    break;
                default:
                    break;
            }

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            
            EditorGUILayout.EndVertical();

        }


        private Vector2 scrollPos;
        private int configTabIndex = 0;

        void OnGUI() {
            
            GUILayout.BeginArea(containerRect);
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            configTabIndex = GUILayout.Toolbar(configTabIndex, new string[] { "基本设置", "授权平台设置", "其他设置" });

            switch (configTabIndex) {
                case 0: // 基本设置 UI 逻辑
                    BasicSettingsUI();
                    break;
                case 1: // 授权平台设置 UI 逻辑
                    if (FunnyConfig.Instance.isMainland) {
                        MainlandAreaUI();
                    }
                    else {
                        OverseaAreaUI();
                    }
                    break;
                default: // 其他设置 UI 逻辑
                    BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
                    switch (selectedBuildTargetGroup) {
                        case BuildTargetGroup.Android:
                            // Android 平台配置项，待处理
                            break;
                        case BuildTargetGroup.iOS:
                            // iOS 平台配置项，待处理
                            break;
                        default:
                            EditorGUILayout.LabelField("当前平台暂未开放，敬请期待！");
                            break;
                    }
                    EditorGUILayout.EndBuildTargetSelectionGrouping();
                    break;
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();

        }

        private void OnInspectorUpdate() {
            Repaint();
        }

    }

}

