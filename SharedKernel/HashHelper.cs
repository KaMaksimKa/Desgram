using System.Security.Cryptography;
using System.Text;
using Desgram.SharedKernel;

namespace SharedKernel
{
    public static class HashHelper
    {
        public static string GetHash(string input)
        {
            using var sha = SHA256.Create();
            var data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BytesHelper.ToStringH2(data);

        }

        public static bool Verify(string input, string hash)
        {
            var hashInput = GetHash(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashInput, hash) == 0;
        }

        
    }
}