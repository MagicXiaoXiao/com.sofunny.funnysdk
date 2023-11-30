using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    internal static partial class FunnyDataStore
    {
        private static string GetkFunnyDataDirName()
        {
            return BridgeConfig.IsMainland ? "FunnySDK" : "FunnySDK_A";
        }

        private static string KFunnyDataRecordFileName => $"fsdk{BridgeConfig.AppID}.funny";
        private static string RecordFilePath => Path.Combine(Application.persistentDataPath, GetkFunnyDataDirName(), KFunnyDataRecordFileName);

        private const int recordCount = 10; // 最大记录条数
        private static readonly List<LoginAccountRecord> AccountRecords;

        internal static SSOToken Current { get; private set; }

        static FunnyDataStore()
        {
            if (!File.Exists(RecordFilePath))
            {
                AccountRecords = new List<LoginAccountRecord>();

                try
                {
                    string dirPath = Path.Combine(Application.persistentDataPath, GetkFunnyDataDirName());
                    Directory.CreateDirectory(dirPath);
                    File.Create(RecordFilePath).Close();
                }
                catch (Exception ex)
                {
                    Logger.LogError($"记录文件创建失败 - {ex.Message}");
                }
            }
            else
            {
                string recordJson = File.ReadAllText(RecordFilePath, Encoding.UTF8);
                AccountRecords = JsonConvert.DeserializeObject<List<LoginAccountRecord>>(recordJson) ?? new List<LoginAccountRecord>();
            }
        }

        // 当前是否存在已登录 Token
        internal static bool HasToken => Current != null;
        // 是否存在记录数据
        internal static bool HasRecord => AccountRecords.Any();

        /// <summary>
        /// 添加账号记录
        /// </summary>
        /// <param name="record"></param>
        internal static void AddAccountRcord(LoginAccountRecord record)
        {
            if (AccountRecords.Contains(record))
            {
                // 已存在则移除旧数据
                AccountRecords.Remove(record);
            }
            // 插入新数据到第一个位置
            AccountRecords.Insert(0, record);

            if (AccountRecords.Count > recordCount)
            {
                // 如当前数据超出上限，则移除末尾数据
                AccountRecords.RemoveAt(recordCount);
            }

            SyncSaveData();
        }

        /// <summary>
        /// 移除账号记录 (根据账号)
        /// </summary>
        /// <param name="account"></param>
        internal static void RemoveAccountRecord(string account)
        {
            var record = AccountRecords.FirstOrDefault(item => item.Account == account);
            if (record is null) return;
            RemoveAccountRecord(record);
        }

        /// <summary>
        /// 移除本地账号记录 (根据账号记录信息)
        /// </summary>
        /// <param name="record"></param>
        internal static void RemoveAccountRecord(LoginAccountRecord record)
        {
            AccountRecords.Remove(record);
            SyncSaveData();
        }

        /// <summary>
        /// 根据账号获取登录记录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal static LoginAccountRecord GetAccountRecord(string account)
        {
            return AccountRecords.FirstOrDefault(item => item.Account == account);
        }


        internal static bool TryGetAccountRecord(string account, out LoginAccountRecord record)
        {
            record = AccountRecords.FirstOrDefault(item => item.Account == account);
            return record != null;
        }

        internal static LoginAccountRecord GetFirstRecord()
        {
            return AccountRecords.FirstOrDefault();
        }

        internal static List<LoginAccountRecord> GetRecordList()
        {
            return AccountRecords;
        }

        internal static void UpdateToken(SSOToken ssoToken)
        {
            Current = ssoToken;
        }

        internal static void UpdateTokenAndRecord(SSOToken ssoToken)
        {
            if (Current is null) return;

            LoginAccountRecord currentRecord = AccountRecords.FirstOrDefault(record => record.Token.RefreshToken == Current.RefreshToken);

            if (currentRecord is null) return;

            currentRecord.Token = ssoToken;
            AddAccountRcord(currentRecord);

            UpdateToken(ssoToken);
        }

        internal static void DeleteToken()
        {
            Current = null;
        }

        #region Save Data Method

        private static void SyncSaveData()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(AccountRecords);

                WriteTextToFile(RecordFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Logger.LogError($"记录写入失败 - {ex.Message}");
            }
        }

        private static void WriteTextToFile(string filePath, string content)
        {
            // 将文本内容编码为字节数组
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            // 文件写入
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                sourceStream.Write(bytes, 0, bytes.Length);
            }

        }

        #endregion
    }
}

