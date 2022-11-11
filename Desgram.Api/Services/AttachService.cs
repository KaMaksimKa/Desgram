using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Desgram.Api.Models.Attach;

namespace Desgram.Api.Services
{
    public class AttachService:IAttachService
    {
        private readonly ApplicationContext _context;
        private readonly string _defaultPathImage = $"attaches";

        public AttachService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<MetadataModel> SaveToTempAsync(IFormFile file)
        {
            var metadata = new MetadataModel()
            {
                Id = Guid.NewGuid(),
                MimeType = file.ContentType,
                Name = file.Name,
                Size = file.Length
            };

            var path = GetTempPathById(metadata.Id);

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

        private string GetTempPathById(Guid id)
        {
           return Path.Combine(Path.GetTempPath(), "Desgram", _defaultPathImage, id.ToString());
        }
        public string MoveFromTempToAttach(MetadataModel model)
        {
            var pathTemp = GetTempPathById(model.Id); ;
            var fileInfoTemp = new FileInfo(pathTemp);

            if (!fileInfoTemp.Exists)
            {
                throw new CustomException("file not found");
            }

            var pathAttach = GetAttachPathById(model.Id);

            var fileInfoAttach = new FileInfo(pathAttach);

            if (fileInfoAttach.Directory is { Exists: false })
            {
                fileInfoAttach.Directory.Create();
            }

            File.Copy(pathTemp, pathAttach, true);

            return pathAttach;
        }

        public string GetAttachPathById(Guid id)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _defaultPathImage, id.ToString());
        }
        public async Task<AttachWithPathModel> GetAttachByIdAsync(Guid id)
        {
            var attach =await _context.Attaches.FirstOrDefaultAsync(x => x.Id == id);
            if (attach == null)
            {
                throw new CustomException("attach not found");
            }

            return new AttachWithPathModel()
            {
                Id = attach.Id,
                FilePath = attach.Path,
                MimeType = attach.MimeType,
                Name = attach.Name
            };
        }

    }
}
