using Desgram.Api.Controllers;
using Desgram.Api.Infrastructure;
using Desgram.Api.Services.Interfaces;
using Desgram.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Desgram.Api.Services
{
    public class UrlService: IUrlService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _accessor;

        public UrlService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor accessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _accessor = accessor;
        }

        public string GetUrlDisplayAttachById(Guid id)
        {
            if (_accessor.ActionContext == null)
            {
                throw new NullReferenceException(nameof(_accessor.ActionContext));
            }
            var urlHelper = _urlHelperFactory.GetUrlHelper(_accessor.ActionContext);

            var url = urlHelper.Action(nameof(AttachController.DisplayAttachById),
                ControllerHelper.NameOfController<AttachController>(), new { id });
            if (url == null)
            {
                throw new NullReferenceException(nameof(url));
            }
            return url;
        }

        public string GetUrlDownloadAttachById(Guid id)
        {
            if (_accessor.ActionContext == null)
            {
                throw new NullReferenceException(nameof(_accessor.ActionContext));
            }
            var urlHelper = _urlHelperFactory.GetUrlHelper(_accessor.ActionContext);

            var url = urlHelper.Action(nameof(AttachController.DownloadAttachById),
                ControllerHelper.NameOfController<AttachController>(), new { id });
            if (url == null)
            {
                throw new NullReferenceException(nameof(url));
            }
            return url;
        }
    }
}
