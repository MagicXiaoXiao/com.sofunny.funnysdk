using System;

namespace SoFunny.FunnySDK
{
    internal class AccountInfo
    {

        private static readonly object _lock = new object();
        private static AccountInfo _instance;

        internal static AccountInfo Current
        {
            get
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new AccountInfo();
                    }
                }

                return _instance;
            }
        }

        private AccountInfo() { }

        internal AccessToken Token;
        internal UserProfile Profile;
        internal BindInfo BindInfo;

        internal void Clear()
        {
            Token = null;
            Profile = null;
            BindInfo = null;
        }

    }
}

