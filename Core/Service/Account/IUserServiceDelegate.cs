using System;

namespace SoFunny.FunnySDK
{
    public interface IUserServiceDelegate
    {
        void OnUserProfileSuccess(UserProfile profile);
        void OnUserProfileFailure(ServiceError error);
    }
}

