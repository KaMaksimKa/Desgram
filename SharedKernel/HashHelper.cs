using System.Security.Cryptography;
using System.Text;

namespace Desgram.SharedKernel
{
    public static class HashHelper
    {
        public static string GetHash(string input)
        {
            using var sha = SHA256.Create();
            var data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            var result = BytesHelper.ToStringH2(data);
            return result;

        }

        public static bool Verify(string input, string hash)
        {
            var hashInput = GetHash(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashInput, hash) == 0;
        }

        
    }
}