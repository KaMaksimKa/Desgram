using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Microsoft.EntityFrameworkCore;
using Desgram.Api.Models.Attach;
using Desgram.Api.Services.ServiceModel;
using Desgram.SharedKernel.Exceptions;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using SkiaSharp;
using System.IO;


namespace Desgram.Api.Services
{
    public class AttachService:IAttachService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IImageEditor _imageEditor;
        private const string DefaultPathImage = $"attaches";

        public AttachService(ApplicationContext context,IMapper mapper,IImageEditor imageEditor)
        {
            _context = context;
            _mapper = mapper;
            _imageEditor = imageEditor;
        }

        public async Task<MetadataModel> SaveToTempAsync(IFormFile file)
        {
            var metadata = new MetadataModel()
            {
                Id = Guid.NewGuid(),
                MimeType = file.ContentType,
                Name = file.Name,
            };

            var path = GetTempPathById(metadata.Id);

            var fileInfo = new FileInfo(path);

            if (fileInfo.Directory is { Exists: false })
            {
                fileInfo.Directory.Create();
            }

            await using var fileStream = fileInfo.Create();

            await file.CopyToAsync(fileStream);


            return metadata;
        }

        public async Task<AttachWithPathModel> GetAttachByIdAsync(Guid id)
        {
            var attach =await _context.Attaches.AsNoTracking()
                .ProjectTo<AttachWithPathModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (attach == null)
            {
                throw new AttachNotFoundException();
            }

            return attach;
        }

        
        public async Task<List<ImageWithPathModel>> FromTempToImage(MetadataModel model)
        {

            var tempFileInfo = new FileInfo(GetTempPathById(model.Id));
            if (!tempFileInfo.Exists)
            {
                throw new AttachNotFoundException();
            }

            await using var tempStream = tempFileInfo.Open(FileMode.Open);

            using var skimageFromTemp = SKImage.FromEncodedData(tempStream);

            if (skimageFromTemp == null)
            {
                throw new FileFormatIsNotSupportedException();
            }

            var imagesEditingInfo = GetImagesPostEditingInfo(skimageFromTemp.Width, skimageFromTemp.Height);

            var convertImages = new List<ImageWithPathModel>();

            foreach (var editingInfo in imagesEditingInfo)
            {
                using var croppedImage = _imageEditor.CropCentrally(skimageFromTemp, editingInfo.ImageProportions);

                using var croppedSkbitmap = SKBitmap.FromImage(croppedImage);

                using var resizedBitmap = croppedSkbitmap.Resize(
                    new SKImageInfo(editingInfo.NewWidth, editingInfo.NewHeight), SKFilterQuality.High);

                await using var bitmapStream = resizedBitmap.Encode(SKEncodedImageFormat.Jpeg, 100).AsStream();

                var imageId = Guid.NewGuid();
                var imagePath = GetAttachPathById(imageId);

                convertImages.Add(new ImageWithPathModel()
                {
                    Id = imageId,
                    Width = resizedBitmap.Width,
                    Height = resizedBitmap.Height,
                    MimeType = "image/jpeg",
                    Path = imagePath
                });

                var imageFileInfo = new FileInfo(imagePath);

                if (imageFileInfo.Directory is { Exists: false })
                {
                    imageFileInfo.Directory.Create();
                }


                await using var attachStream = imageFileInfo.Create();

                await bitmapStream.CopyToAsync(attachStream);
            }

            return convertImages;
        }
        
        public async Task<bool> IsImage(MetadataModel model)
        {

            var tempPath = GetTempPathById(model.Id);
            await using var tempStream = new FileStream(tempPath, FileMode.Open);

            using var skimageFromTemp = SKImage.FromEncodedData(tempStream);

            return skimageFromTemp != null;
        }

        /// <summary>
        /// Returns information about necessary changing image
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private List<ImageEditingInfo> GetImagesPostEditingInfo(int width, int height)
        {

            /*var proportions = _imageEditor.GetImageProportions(width, height);*/

            /*Пока что все изображения будут обрезаться 1:1, в будущем это поправиться*/
            var proportions = new ImageProportions(1, 1);

            var imagesEditingInfo = new List<ImageEditingInfo>();



            foreach (var newWidth in ImageWidths.Widths)
            {
                if (newWidth <= width)
                {
                    imagesEditingInfo.Add(new ImageEditingInfo()
                    {
                        ImageProportions = proportions,
                        NewWidth = newWidth,
                        NewHeight = newWidth / proportions.CountPartsWidth * proportions.CountPartsHeight
                    });
                }

            }

            return imagesEditingInfo;
        }

        private string GetTempPathById(Guid id)
        {
            return Path.Combine(Path.GetTempPath(), "Desgram", DefaultPathImage, id.ToString());
        }

        private string GetAttachPathById(Guid id)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), DefaultPathImage, id.ToString());
        }


        
    }

    public record ImageProportions(int CountPartsWidth, int CountPartsHeight);

    public class ImageEditingInfo
    {
        public int NewWidth { get; set; }
        public int NewHeight { get; set; }
        public ImageProportions ImageProportions { get; set; } = null!;
    }



}
