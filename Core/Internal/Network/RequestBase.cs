﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal abstract class RequestBase
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        internal virtual string BaseURL => BridgeConfig.BaseURL;

        /// <summary>
        /// 请求方法
        /// </summary>
        internal abstract HttpMethod Method { get; }

        /// <summary>
        /// 接口地址
        /// </summary>
        internal abstract string Path { get; }

        /// <summary>
        /// 请求超时时间
        /// </summary>
        internal virtual TimeSpan TimeOut => TimeSpan.FromSeconds(15);

        /// <summary>
        /// 请求参数表
        /// </summary>
        /// <returns></returns>
        internal virtual Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 是否携带 AppID，默认 false
        /// </summary>
        internal virtual bool AppID => false;

        /// <summary>
        /// 是否携带当前设备信息，默认 false
        /// </summary>
        internal virtual bool DeviceInfo => false;

        /// <summary>
        /// 设置 Token 头
        /// </summary>
        internal virtual string Token => string.Empty;

    }
}

