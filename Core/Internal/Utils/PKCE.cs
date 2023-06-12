using System;
using System.Security.Cryptography;
using System.Text;

namespace SoFunny.FunnySDK.Internal
{
    internal class PKCE
    {
        internal readonly string CodeChallengeMethod = "S256";

        internal string CodeVerifier { get; private set; }

        internal string CodeChallenge => GenerateCodeChallenge(CodeVerifier);

        internal PKCE()
        {
            byte[] randomBytes = new byte[32];
            Random random = new Random();
            random.NextBytes(randomBytes);
            CodeVerifier = Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        static string GenerateCodeChallenge(string codeVerifier)
        {
            var asciiValue = Encoding.ASCII.GetBytes(codeVerifier);

            var sha256 = SHA256.Create();

            var sha256Value = sha256.ComputeHash(asciiValue);

            return Convert.ToBase64String(sha256Value).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}

