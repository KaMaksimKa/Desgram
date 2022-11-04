using Desgram.Api.Models;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAttachService
    {
        public Task<MetadataModel> SaveToTempAsync(IFormFile file);

        public string MoveFromTempToAttach(MetadataModel model);

    }
}
