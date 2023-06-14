using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SoFunny.FunnySDK
{
    internal static class StringExtensions
    {
        /// <summary>
        /// 进行 MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string ToMD5(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取 URL 中的参数
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        internal static string GetQuery(this string uriString, string paramName)
        {
            Uri uri = new Uri(uriString);

            string paramValue = string.Empty;

            if (!string.IsNullOrEmpty(paramName))
            {
                string query = uri.Query.TrimStart('?');

                foreach (string param in query.Split('&'))
                {
                    string[] keyValue = param.Split('=');

                    if (keyValue.Length == 2 && keyValue[0] == paramName)
                    {
                        paramValue = keyValue[1];
                        break;
                    }
                }
            }

            return paramValue;
        }

        /// <summary>
        /// 是否符合国内手机号匹配规则
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsMatchPhone(this string value)
        {
            return Regex.IsMatch(value, @"^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\d{8}$");
        }

        /// <summary>
        /// 是否符合邮箱匹配规则
        /// </summary>
        /// <param name="valie"></param>
        /// <returns></returns>
        internal static bool IsMatchEmail(this string value)
        {
            return Regex.IsMatch(value, @"^[a-z0-9A-Z]+[-|a-z0-9A-Z._]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?.)+[a-z]{2,}$");
        }

    }
}

