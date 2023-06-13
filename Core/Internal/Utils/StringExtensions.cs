using System;
using System.Security.Cryptography;
using System.Text;

namespace SoFunny.FunnySDK.Internal
{
    internal static class StringExtensions
    {
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

            //string[] keyValuePairs = queryString.TrimStart('?').Split('&');

            //foreach (string keyValuePair in keyValuePairs)
            //{
            //    string[] keyValue = keyValuePair.Split('=');
            //    if (keyValue.Length == 2 && keyValue[0] == key)
            //    {
            //        return keyValue[1];
            //    }
            //}

            //return paramValue;
        }
    }
}

