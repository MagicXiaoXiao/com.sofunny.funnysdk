using System;

namespace SoFunny.FunnySDKPreview
{

    internal interface INativeResultDecodeHandler
    {
        void DecodeResult(string taskID, NativeResult result);
    }
}

