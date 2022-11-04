using Desgram.Api.Services.Interfaces;
using Desgram.Api.Models;
using Desgram.SharedKernel.Exceptions;

namespace Desgram.Api.Services
{
    public class AttachService:IAttachService
    {
        private readonly string _defaultPathImage = $"attaches";


        public async Task<MetadataModel> SaveToTempAsync(IFormFile file)
        {
            var metadata = new MetadataModel()
            {
                Id = Guid.NewGuid(),
                MimeType = file.ContentType,
                Name = file.Name,
            };

            var path = Path.Combine(Path.GetTempPath(), "Desgram",_defaultPathImage,metadata.Id.ToString());

            var fileInfo = new FileInfo(path);

            if (fileInfo.Directory is { Exists: false })
            {
                fileInfo.Directory.Create();
            }

            using (var fileStream = new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return metadata;
        }

        public string MoveFromTempToAttach(MetadataModel model)
        {
            var pathTemp =  Path.Combine(Path.GetTempPath(), "Desgram", _defaultPathImage, model.Id.ToString());
            var fileInfoTemp = new FileInfo(pathTemp);

            if (!fileInfoTemp.Exists)
            {
                throw new CustomException("file not found");
            }

            var pathAttach = Path.Combine(Directory.GetCurrentDirectory(),_defaultPathImage,model.Id.ToString());
            var fileInfoAttach = new FileInfo(pathAttach);

            if (fileInfoAttach.Directory is { Exists: false })
            {
                fileInfoAttach.Directory.Create();
            }

            File.Copy(pathTemp, pathAttach, true);

            return pathAttach;
        }


        /*public string SaveImage(IFormFile file)
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
        }*/

        
    }
}
