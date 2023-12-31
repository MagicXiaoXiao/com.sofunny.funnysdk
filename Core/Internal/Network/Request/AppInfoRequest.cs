﻿using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SoFunny.FunnySDK.Internal
{
    internal class AppInfoRequest : RequestBase
    {
        internal AppInfoRequest()
        {
        }

        internal override HttpMethod Method => HttpMethod.Get;

        internal override string Path => "/app/init_info";

        internal override bool AppID => true;

        internal override Dictionary<string, object> Parameters()
        {
            return new Dictionary<string, object>()
            {
                {"device_category","pc" }
            };
        }

    }
}

