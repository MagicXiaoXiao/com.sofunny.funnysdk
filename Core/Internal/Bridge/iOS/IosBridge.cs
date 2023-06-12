using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.Internal
{
    internal class IosBridge : IBridgeServiceBase
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SendVerificationCode(string account, CodeAction codeAction, CodeCategory codeCategory, ServiceCompletedHandler<VoidObject> handler)
        {
            throw new NotImplementedException();
        }
    }
}

