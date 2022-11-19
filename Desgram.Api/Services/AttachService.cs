using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Microsoft.EntityFrameworkCore;
using Desgram.Api.Models.Attach;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using SkiaSharp;


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


            await using var fileStream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(fileStream);

            EditImage(fileStream);

            return metadata;
        }

        public string MoveFromTempToAttach(MetadataModel model)
        {
            var pathTemp = GetTempPathById(model.Id);
            var fileInfoTemp = new FileInfo(pathTemp);

            if (!fileInfoTemp.Exists)
            {
                throw new AttachNotFoundException();
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
                throw new AttachNotFoundException();
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

        private void EditImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var original = SKBitmap.Decode(stream);

            var h = original.Height;
            var w = original.Width;

            var resized = original.Resize(new SKImageInfo(original.Width, original.Height),SKFilterQuality.High);
            var image = SKImage.FromBitmap(resized);

            
        }
    }
}
