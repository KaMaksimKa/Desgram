using Desgram.Api.Services.Interfaces;
using Desgram.Api.Models;
using Desgram.DAL;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AttachModel> GetAttachById(Guid id)
        {
            var attach =await _context.Attaches.FirstOrDefaultAsync(x => x.Id == id);
            if (attach == null)
            {
                throw new CustomException("attach not found");
            }

            return new AttachModel()
            {
                FilePath = attach.Path,
                MimeType = attach.MimeType,
                Name = attach.Name
            };
        }

    }
}
