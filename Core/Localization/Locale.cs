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
        internal static bool CurrentLanguageHasBeenSet = false;

        private static string currentLanguage;
        private static TextAsset currentLocalizationText;


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
            if (language == SystemLanguage.Chinese || language == SystemLanguage.ChineseSimplified || language == SystemLanguage.ChineseTraditional)
            {
                CurrentLanguage = "Chinese";
            }
            else
            {
                CurrentLanguage = language.ToString();
            }

            PlayerLanguage = language;

            FunnyLocalize[] allTexts = GameObject.FindObjectsOfType<FunnyLocalize>();

            for (int i = 0; i < allTexts.Length; i++)
            {
                allTexts[i].UpdateLocale();
            }
        }

        public static string LoadText(string key)
        {
            if (CurrentLanguageStrings.ContainsKey(key))
            {
                return CurrentLanguageStrings[key];
            }

            return key;
        }
    }
}


