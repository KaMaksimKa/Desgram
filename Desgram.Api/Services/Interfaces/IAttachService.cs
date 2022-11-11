using Desgram.Api.Models.Attach;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAttachService
    {
        public Task<MetadataModel> SaveToTempAsync(IFormFile file);

        public string MoveFromTempToAttach(MetadataModel model);

        public Task<AttachWithPathModel> GetAttachByIdAsync(Guid id);

    }
}
