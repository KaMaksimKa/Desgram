using Desgram.Api.Models.Attach;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUrlService
    {
        public string GetUrlDisplayAttachById(Guid id);
        public string GetUrlDownloadAttachById(Guid id);

    }
}
