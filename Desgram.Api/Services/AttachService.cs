using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using Desgram.Api.Models.Attach;

namespace Desgram.Api.Services
{
    public class AttachService:IAttachService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly string _defaultPathImage = $"attaches";

        public AttachService(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public string MoveFromTempToAttach(MetadataModel model)
        {
            var pathTemp = GetTempPathById(model.Id);
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

        public async Task<AttachWithPathModel> GetAttachByIdAsync(Guid id)
        {
            var attach =await _context.Attaches
                .ProjectTo<AttachWithPathModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (attach == null)
            {
                throw new CustomException("attach not found");
            }

            return attach;
        }

        private string GetTempPathById(Guid id)
        {
            return Path.Combine(Path.GetTempPath(), "Desgram", _defaultPathImage, id.ToString());
        }

        private string GetAttachPathById(Guid id)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _defaultPathImage, id.ToString());
        }
    }
}
