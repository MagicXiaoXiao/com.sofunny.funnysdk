using System;

namespace SoFunny.FunnySDK.Internal
{
    public interface IRequestCompletedHandler
    {
        void OnSuccessHandler(string body);

        void OnFailureHandler(ServiceError error);
    }
}

