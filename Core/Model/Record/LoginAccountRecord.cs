using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class LoginAccountRecord : IEquatable<LoginAccountRecord>, IEqualityComparer<LoginAccountRecord>
    {

        [JsonProperty("account")]
        internal string Account;
        [JsonProperty("token")]
        internal SSOToken Token;
        [JsonProperty("symbol_count")]
        internal int SymbolCount = 0;

        internal string Pwd
        {
            get
            {
                return new string('*', SymbolCount);
            }
        }

        [JsonConstructor]
        internal LoginAccountRecord(string account)
        {
            Account = account;
        }

        internal LoginAccountRecord(string account, SSOToken ssoToken)
        {
            Account = account;
            Token = ssoToken;
            SymbolCount = 0;
        }

        public bool Equals(LoginAccountRecord other)
        {
            return Account == other.Account;
        }

        public bool Equals(LoginAccountRecord x, LoginAccountRecord y)
        {
            return x.Account == y.Account;
        }

        public int GetHashCode(LoginAccountRecord obj)
        {
            if (obj == null)
                return 0;

            return obj.Account.GetHashCode();
        }

    }
}

