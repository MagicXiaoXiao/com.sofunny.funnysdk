using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    internal static class Locale
    {

        const string STR_LOCALIZATION_KEY = "com.funnysdk.locale";
        const string STR_LOCALIZATION_PREFIX = "FunnySDK/Localization/";

        internal static Dictionary<string, string> CurrentLanguageStrings = new Dictionary<string, string>();
        private static readonly Dictionary<SystemLanguage, string> languageCodes = new Dictionary<SystemLanguage, string>();
        internal static bool CurrentLanguageHasBeenSet = false;

        private static string currentLanguage;
        private static TextAsset currentLocalizationText;

        static Locale()
        {
            languageCodes[SystemLanguage.Chinese] = "zh-CN";// 中国-大陆
            languageCodes[SystemLanguage.ChineseSimplified] = "zh-CN"; // 中国-大陆
            languageCodes[SystemLanguage.ChineseTraditional] = "zh-HK"; // 中国-香港
            languageCodes[SystemLanguage.English] = "en-US";// 美国
            languageCodes[SystemLanguage.Indonesian] = "id-ID"; // 印度尼西亚
            languageCodes[SystemLanguage.Vietnamese] = "vi-VN"; // 越南
            languageCodes[SystemLanguage.Thai] = "th-TH"; // 泰国
        }


        /// <summary>
        /// This sets the current language. It expects a standard .Net CultureInfo.Name format
        /// </summary>
        internal static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    currentLanguage = value;
                    currentLocalizationText = Resources.Load<TextAsset>(STR_LOCALIZATION_PREFIX + currentLanguage);
                    if (currentLocalizationText == null)
                    {
                        Debug.LogWarningFormat("Missing locale '{0}', loading English.", currentLanguage);
                        currentLanguage = SystemLanguage.English.ToString();
                        currentLocalizationText = Resources.Load<TextAsset>(STR_LOCALIZATION_PREFIX + currentLanguage);
                    }
                    if (currentLocalizationText != null)
                    {
                        // We wplit on newlines to retrieve the key pairs
                        string[] lines = currentLocalizationText.text.Split(new string[] { "\r\n", "\n\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                        CurrentLanguageStrings.Clear();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] pairs = lines[i].Split(new char[] { '\t', '=' }, 2);
                            if (pairs.Length == 2)
                            {
                                CurrentLanguageStrings.Add(pairs[0].Trim(), pairs[1].Trim());
                            }
                        }
                    }
                    else
                    {
                        Logger.LogWarning($"Locale Language '{currentLanguage}' not found!");
                    }
                }
            }
        }

        internal static string GetLanguageCode(SystemLanguage language)
        {
            if (languageCodes.ContainsKey(language))
            {
                return languageCodes[language];
            }
            else
            {
                return languageCodes[SystemLanguage.English];
            }
        }

        /// <summary>
        /// The player language. If not set in PlayerPrefs then returns Application.systemLanguage
        /// </summary>
        internal static SystemLanguage PlayerLanguage
        {
            get
            {
                return (SystemLanguage)PlayerPrefs.GetInt(STR_LOCALIZATION_KEY, (int)Application.systemLanguage);
            }
            set
            {
                PlayerPrefs.SetInt(STR_LOCALIZATION_KEY, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static void SetCurrentLanguage(SystemLanguage language)
        {
            CurrentLanguage = GetLanguageCode(language);
            PlayerLanguage = language;

            FunnyLocalize[] allTexts = GameObject.FindObjectsOfType<FunnyLocalize>();

            for (int i = 0; i < allTexts.Length; i++)
            {
                allTexts[i].UpdateLocale();
            }
        }

        public static string LoadText(string key)
        {
            if (!CurrentLanguageHasBeenSet)
            {
                SetCurrentLanguage(PlayerLanguage);
            }

            if (CurrentLanguageStrings.ContainsKey(key))
            {
                return CurrentLanguageStrings[key];
            }

            return key;
        }
    }
}


