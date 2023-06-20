using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK.Editor
{

    public class SettingsWindow : EditorWindow
    {

        [MenuItem("SoFunnySDK/FunnySDK Settings", priority = 1)]
        public static void AttackWindowMethod()
        {

            if (!FunnyEditorConfig.CheckConfigFile())
            {
                FunnyEditorConfig.CreateConfigFile();
            }

            SettingsWindow.Show();
        }

        [MenuItem("SoFunnySDK/Create Config", priority = 99)]
        public static void OnCreateSOConfig()
        {
            if (FunnyEditorConfig.CheckConfigFile())
            {
                if (EditorUtility.DisplayDialog("提示", "当前项目 Assets/Resources/FunnySDK 目录下已存在配置文件，是否重新创建?", "重新创建", "取消"))
                {
                    FunnyEditorConfig.CreateConfigFile();
                }
            }
            else
            {
                FunnyEditorConfig.CreateConfigFile();

                EditorUtility.DisplayDialog("创建成功", "配置文件已创建完毕", "好的");
            }
        }

        private static FunnySDKConfig sdkConfig;

        // Add menu named "My Window" to the Window menu
        internal static new void Show()
        {

            sdkConfig = FunnyEditorConfig.GetConfig();

            if (sdkConfig == null)
            {
                EditorUtility.DisplayDialog("出错", "FunnySDK 插件包缺少配置信息，请重新安装此插件", "好的");
            }
            else
            {
                // Get existing open window or if none, make a new one:
                var window = EditorWindow.GetWindow<SettingsWindow>("FunnySDK Settings");
                window.ShowModal();
            }
        }

        private void OnDestroy()
        {
            FunnyEditorConfig.SyncData();
        }

        float containerPadding = 12;
        Rect containerRect => new Rect(
            x: containerPadding,
            y: containerPadding,
            width: position.width - containerPadding * 2,
            height: position.height - containerPadding * 2
        );

        private GUIStyle appIDLabelStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.boldLabel);
                return style;
            }
        }


        private GUIStyle funnyToggleStyle
        {
            get
            {
                var style = new GUIStyle()
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    clipping = TextClipping.Overflow,
                    alignment = TextAnchor.MiddleLeft
                };
                style.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;

                return style;
            }
        }

        private bool mainlandValue
        {
            get
            {
                return sdkConfig.IsMainland;
            }
            set
            {
                sdkConfig.IsMainland = value;
            }
        }

        private bool overseaValue
        {
            get
            {
                return !sdkConfig.IsMainland;
            }
            set
            {
                sdkConfig.IsMainland = !value;
            }
        }


        private bool isWebUI
        {
            get
            {
                return sdkConfig.IsWebUI;
            }
            set
            {
                sdkConfig.IsWebUI = value;
            }
        }

        void BasicSettingsUI()
        {

            var boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 10, 10)
            };

            EditorGUILayout.BeginVertical(boxStyle);

            EditorGUILayout.BeginHorizontal();
            overseaValue = EditorGUILayout.ToggleLeft("海外", overseaValue, funnyToggleStyle, GUILayout.Width(60));
            mainlandValue = EditorGUILayout.ToggleLeft("国内", mainlandValue, funnyToggleStyle, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AppID：", appIDLabelStyle, GUILayout.MaxWidth(50));
            sdkConfig.AppID = EditorGUILayout.TextField(sdkConfig.AppID, GUILayout.MaxWidth(200));
            EditorGUILayout.LabelField("*必填", GUILayout.MaxWidth(44));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            isWebUI = EditorGUILayout.ToggleLeft("勾选登录主流程 UI 为 Web 反之为 UGUI", isWebUI, funnyToggleStyle, GUILayout.Width(60));

            if (EditorGUI.actionKey)
            {
                EditorGUILayout.LabelField($"当前包名为：", GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField($"{PlayerSettings.applicationIdentifier}", GUILayout.ExpandWidth(false));
            }

            EditorGUILayout.EndVertical();

        }

        int mlSelectIndex = 0;
        int mlSelectChangeIndex = 0;

        void MainlandAreaUI()
        {
            // 国内配置显示逻辑
            var boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
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

            switch (mlSelectIndex)
            {
                case 0: // TapTap
                    EditorGUILayout.LabelField("TapTap Client ID");
                    sdkConfig.TapTap.clientID = EditorGUILayout.TextField(sdkConfig.TapTap.clientID);
                    EditorGUILayout.LabelField("TapTap Client Token");
                    sdkConfig.TapTap.clientToken = EditorGUILayout.TextField(sdkConfig.TapTap.clientToken);
                    EditorGUILayout.LabelField("TapTap Server URL");
                    sdkConfig.TapTap.serverURL = EditorGUILayout.TextField(sdkConfig.TapTap.serverURL);
                    EditorGUILayout.Separator();
                    sdkConfig.TapTap.isTapBeta = EditorGUILayout.ToggleLeft("TapTap 小号测试", sdkConfig.TapTap.isTapBeta, funnyToggleStyle);
                    EditorGUILayout.Separator();
                    sdkConfig.TapTap.isBonfire = EditorGUILayout.ToggleLeft("篝火资格校验逻辑", sdkConfig.TapTap.isBonfire, funnyToggleStyle);
                    break;
                case 1: // QQ
                    EditorGUILayout.LabelField("QQ App ID");
                    sdkConfig.QQ.appID = EditorGUILayout.TextField(sdkConfig.QQ.appID);
                    EditorGUILayout.LabelField("QQ Universal Link");
                    sdkConfig.QQ.universalLink = EditorGUILayout.TextField(sdkConfig.QQ.universalLink);
                    break;
                case 2: // 微信
                    var wxTips = new GUIStyle();
                    wxTips.normal.textColor = EditorGUIUtility.isProSkin ? Color.yellow : Color.red;
                    EditorGUILayout.LabelField("如需接入微信，则 AppID 和 UniversalLink 参数必须填写", wxTips);
                    EditorGUILayout.LabelField("WeChat App ID");
                    sdkConfig.WeChat.appID = EditorGUILayout.TextField(sdkConfig.WeChat.appID);
                    EditorGUILayout.LabelField("WeChat Universal Link");
                    sdkConfig.WeChat.universalLink = EditorGUILayout.TextField(sdkConfig.WeChat.universalLink);
                    break;
                case 3: // AppleSignIn
                    EditorGUILayout.BeginHorizontal();
                    sdkConfig.Apple.mainlandEnable = EditorGUILayout.ToggleLeft("勾选则表示开启 SignIn With Apple", sdkConfig.Apple.mainlandEnable);
                    EditorGUILayout.EndHorizontal();
                    break;
                default:
                    break;
            }

            EditorGUILayout.EndVertical();
        }

        int osSelectIndex = 0;
        int osSelectChangeIndex = 0;

        void OverseaAreaUI()
        {
            // 海外配置显示逻辑
            var boxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 10, 10)
            };
            EditorGUILayout.BeginVertical(boxStyle);
            osSelectIndex = GUILayout.Toolbar(osSelectIndex, new string[] { "Google", "Facebook", "AppleSignIn", "Twitter" });

            EditorGUILayout.Separator();

            if (osSelectIndex != osSelectChangeIndex)
            {
                osSelectChangeIndex = osSelectIndex;
                EditorGUI.FocusTextInControl(null);
            }

            switch (osSelectIndex)
            {
                case 0: // Google
                    EditorGUILayout.LabelField("Google IDToken");
                    sdkConfig.Google.idToken = EditorGUILayout.TextField(sdkConfig.Google.idToken);
                    break;
                case 1: // Facebook
                    EditorGUILayout.LabelField("Facebook App ID");
                    sdkConfig.Facebook.appID = EditorGUILayout.TextField(sdkConfig.Facebook.appID);
                    EditorGUILayout.LabelField("Facebook Client Token");
                    sdkConfig.Facebook.clientToken = EditorGUILayout.TextField(sdkConfig.Facebook.clientToken);
                    EditorGUILayout.Space();
                    sdkConfig.Facebook.trackEnable = EditorGUILayout.ToggleLeft("启用 Facebook 自动数据收集", sdkConfig.Facebook.trackEnable);
                    break;
                case 2: // AppleSignIn
                    EditorGUILayout.BeginHorizontal();
                    sdkConfig.Apple.overseaEnable = EditorGUILayout.ToggleLeft("勾选则表示开启 SignIn With Apple", sdkConfig.Apple.overseaEnable);
                    EditorGUILayout.EndHorizontal();
                    break;
                case 3: // Twitter
                    EditorGUILayout.LabelField("Twitter Consumer Key");
                    sdkConfig.Twitter.consumerKey = EditorGUILayout.TextField(sdkConfig.Twitter.consumerKey);
                    EditorGUILayout.LabelField("Twitter Consumer Secret");
                    sdkConfig.Twitter.consumerSecret = EditorGUILayout.TextField(sdkConfig.Twitter.consumerSecret);
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

        void OnGUI()
        {

            GUILayout.BeginArea(containerRect);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            configTabIndex = GUILayout.Toolbar(configTabIndex, new string[] { "基本设置", "授权平台设置", "其他设置" });

            switch (configTabIndex)
            {
                case 0: // 基本设置 UI 逻辑
                    BasicSettingsUI();
                    break;
                case 1: // 授权平台设置 UI 逻辑
                    if (sdkConfig.IsMainland)
                    {
                        MainlandAreaUI();
                    }
                    else
                    {
                        OverseaAreaUI();
                    }
                    break;
                default: // 其他设置 UI 逻辑
                    BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
                    switch (selectedBuildTargetGroup)
                    {
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

        private void OnInspectorUpdate()
        {
            Repaint();
        }

    }

}

