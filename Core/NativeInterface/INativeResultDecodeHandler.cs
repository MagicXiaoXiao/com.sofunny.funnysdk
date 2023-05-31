using System;

namespace SoFunny.FunnySDK
{

    internal interface INativeResultDecodeHandler {
        void DecodeResult(string taskID, NativeResult result);
    }
}

