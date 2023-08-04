using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SoFunny.FunnySDK.Internal
{
    internal class Track
    {
        internal readonly string Name;
        private Dictionary<string, object> Data;

        private Track(string name)
        {
            Name = name;
            Data = new Dictionary<string, object>();
        }

        internal static Track Event(string name)
        {
            return new Track(name);
        }
        /// <summary>
        /// 添加单条属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal Track Add(string key, object value)
        {
            Data.Add(key, value);
            return this;
        }

        /// <summary>
        /// 覆盖添加属性数据
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        internal Track AddData(Dictionary<string, object> dic)
        {
            Data = dic;
            return this;
        }

        internal string JsonData()
        {
            return JsonConvert.SerializeObject(Data);
        }

    }
}

