using Desgram.Api.Services.Interfaces;
using System.IO;
using Desgram.SharedKernel;

namespace Desgram.Api.Services
{
    public class FileService:IFileService
    {
        private readonly string _defaultPathImage = $"Data/Images";

        public string SaveImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            stream.Read(bytes, 0, (int)file.Length);
            var path = Path.Combine(_defaultPathImage, FileHelper.GetDir(),
                FileHelper.GetMD5Hash(bytes) + Path.GetExtension(file.FileName));

            if (Path.GetDirectoryName(path) is { } directory)
            {
                Directory.CreateDirectory(directory);
            }
            
            using var streamWriter = new FileStream(path,FileMode.OpenOrCreate);
            streamWriter.Write(bytes);
            return path;
            
        }
    }
}
