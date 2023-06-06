using System;

namespace SoFunny.FunnySDK.Internal
{
    public interface ILoginResultHandler
    {
        void OnLoginSuccessHandler();
        void OnLoginCancelHandler();
        void OnLoginFailureHandler();
    }
}

