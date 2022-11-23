using AutoMapper;
using Desgram.Api.Models.Attach;
using Desgram.Api.Services.Interfaces;

namespace Desgram.Api.Mapper
{
    public class AttachModelMapperAction: IMappingAction<AttachWithUrlModel, AttachWithUrlModel>
    {
        private readonly IUrlService _urlService;

        public AttachModelMapperAction(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public void Process(AttachWithUrlModel source, AttachWithUrlModel destination, ResolutionContext context)
        {
            destination.Url = _urlService.GetUrlDisplayAttachById(source.Id);
        }
    }
}
