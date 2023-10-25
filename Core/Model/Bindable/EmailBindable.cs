using System;

namespace SoFunny.FunnySDK
{
    internal class EmailBindable : IBindable
    {
        public readonly string Email;
        public readonly string Password;
        public readonly string Code;

        internal EmailBindable(string email, string password, string code)
        {
            this.Email = email;
            this.Password = password;
            this.Code = code;
        }

        public string Flag => "email";

    }
}

