using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel
{
    public static class FileHelper
    {
        public static string GetMD5Hash(byte[] bytes)
        {
            using var md5 = MD5.Create();
            var data = md5.ComputeHash(bytes);

            return BytesHelper.ToStringH2(data);
        }

        public static string GetDir()
        {
            var random = new Random();

            using var md5 = MD5.Create();
            var data = md5.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks));

            var stringData = BytesHelper.ToStringH2(data);

            var firstPart = stringData.Substring(random.Next(stringData.Length-1),2);

            data = md5.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks));

            stringData = BytesHelper.ToStringH2(data);

            var secondPart = stringData.Substring(random.Next(stringData.Length-1),2);

            return $"{firstPart}/{secondPart}";
        }
    }
}
