using System;

namespace SoFunny.FunnySDKPreview
{

    internal interface NativeCallMethodInterface
    {
        void Call(string method);
        void Call(string method, NativeParameter parameter);
        void Call<T>(string method, NativeParameter parameter);

        T CallReturn<T>(string method);
        T CallReturn<T>(string method, NativeParameter parameter);

        void RegisterListener();
    }
}