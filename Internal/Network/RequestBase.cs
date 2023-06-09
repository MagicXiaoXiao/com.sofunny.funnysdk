using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    public abstract class RequestBase
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public virtual string BaseURL => "http://10.30.30.93:9080/v1";

        /// <summary>
        /// 请求方法
        /// </summary>
        public abstract HttpMethod Method { get; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public abstract string Path { get; }

        /// <summary>
        /// 请求超时时间
        /// </summary>
        public virtual TimeSpan TimeOut => TimeSpan.FromSeconds(15);

        /// <summary>
        /// 请求参数表
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 是否携带 AppID，默认 false
        /// </summary>
        public virtual bool AppID => false;

    }
}

