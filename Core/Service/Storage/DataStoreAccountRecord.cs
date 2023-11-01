using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal static partial class FunnyDataStore
    {

        private static readonly string kFunnySDKAccountHistory = $"funnysdk.account.history{BridgeConfig.AppID}";

        private const char splitFlag = ';';
        private const int recordCount = 5;

        private static List<string> GetHistoryArray(string key)
        {
            string historyValues = PlayerPrefs.GetString(key, "");

            if (string.IsNullOrEmpty(historyValues))
            {
                return new List<string>();
            }

            return historyValues.Split(splitFlag).ToList();
        }

        private static void AddHistory(string key, string value)
        {
            if (string.IsNullOrEmpty(value)) { return; }

            List<string> historyList = GetHistoryArray(key);

            if (historyList.Contains(value))
            {
                historyList.Remove(value);
            }

            historyList.Insert(0, value);

            if (historyList.Count > recordCount)
            {
                historyList.RemoveAt(recordCount);
            }

            string newValues = string.Join(splitFlag.ToString(), historyList);

            SaveHistory(key, newValues);
        }

        private static void SaveHistory(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                PlayerPrefs.DeleteKey(key);
            }
            else
            {
                PlayerPrefs.SetString(key, value);
            }

            PlayerPrefs.Save();
        }

        internal static void RemoveAccount(string value)
        {
            if (string.IsNullOrEmpty(value)) { return; }

            List<string> historyList = GetHistoryArray(kFunnySDKAccountHistory);

            if (historyList.Contains(value))
            {
                historyList.Remove(value);

                string newValues = string.Join(splitFlag.ToString(), historyList);
                SaveHistory(kFunnySDKAccountHistory, newValues);
            }
        }

        internal static bool HasRecord
        {
            get
            {
                return PlayerPrefs.HasKey(kFunnySDKAccountHistory);
            }
        }

        internal static List<string> GetAccountHistory()
        {
            return GetHistoryArray(kFunnySDKAccountHistory);
        }

        internal static void AddAccountHistory(string account)
        {
            AddHistory(kFunnySDKAccountHistory, account);
        }

    }
}

