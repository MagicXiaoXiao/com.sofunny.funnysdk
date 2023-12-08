using System;

namespace SoFunny.FunnySDK
{
    internal class PhoneBindable : IBindable
    {
        public readonly string PhoneNumber;
        public readonly string Code;

        internal PhoneBindable(string phone, string code)
        {
            PhoneNumber = phone;
            Code = code;
        }

        public string Flag => "phone";
    }
}

