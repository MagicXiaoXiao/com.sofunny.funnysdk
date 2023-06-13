using System;
using SoFunny.FunnySDK.UIModule;

namespace SoFunny.FunnySDK
{
    public interface IFunnyLoginAPI
    {
        void StartFlow(ILoginServiceDelegate serviceDelegate);
        void GetUserProfile(IUserServiceDelegate serviceDelegate);
        void Logout();
    }
}

