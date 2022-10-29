using Desgram.Api.Services.Interfaces;
using System.IO;
using Desgram.SharedKernel;
using System.Security.Cryptography;

namespace Desgram.Api.Services
{
    public class FileService:IFileService
    {
        private readonly string _defaultPathImage = $"Data/Images";

        public string SaveImage(IFormFile file)
        {
            var path = GetPath(file);

            SaveImage(path, file);
           
            return path;
        }


        private string GetPath(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            stream.Read(bytes, 0, (int)file.Length);
            var path = Path.Combine(_defaultPathImage, GetDir(),
                GetMd5Hash(bytes) + Path.GetExtension(file.FileName));
            return path;
        }

        private void SaveImage(string path,IFormFile file)
        {
            using var stream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            stream.Read(bytes, 0, (int)file.Length);
            if (Path.GetDirectoryName(path) is { } directory)
            {
                Directory.CreateDirectory(directory);
            }

            using var streamWriter = new FileStream(path, FileMode.OpenOrCreate);
            streamWriter.Write(bytes);
        }

        private string GetMd5Hash(byte[] bytes)
        {
            using var md5 = MD5.Create();
            var data = md5.ComputeHash(bytes);

            return BytesHelper.ToStringH2(data);
        }

        private string GetDir()
        {
            var random = new Random();

            using var md5 = MD5.Create();
            var data = md5.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks));

            var stringData = BytesHelper.ToStringH2(data);

            var firstPart = stringData.Substring(random.Next(stringData.Length - 1), 2);

            data = md5.ComputeHash(BitConverter.GetBytes(DateTime.Now.Ticks));

            stringData = BytesHelper.ToStringH2(data);

            var secondPart = stringData.Substring(random.Next(stringData.Length - 1), 2);

            return $"{firstPart}/{secondPart}";
        }
    }
}
