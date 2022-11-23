using Desgram.Api.Models.Attach;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAttachService
    {
        public Task<MetadataModel> SaveToTempAsync(IFormFile file);

        public Task<AttachWithPathModel> GetAttachByIdAsync(Guid id);

        /// <summary>
        /// Берет аттач из времменной папки и делает необходимые преобразование после чего сохраняет
        /// все версии картинки в папку с атачами.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<List<ImageWithPathModel>> FromTempToImage(MetadataModel model);

        public Task<bool> IsImage(MetadataModel model);
    }
}
