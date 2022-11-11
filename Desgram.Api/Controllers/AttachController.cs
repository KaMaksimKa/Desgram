using Desgram.Api.Models.Attach;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachController : ControllerBase
    {
        private readonly IAttachService _attachService;

        public AttachController(IAttachService attachService)
        {
            _attachService = attachService;
        }

        [HttpPost]
        public async Task<List<MetadataModel>> UploadFiles(List<IFormFile> files)
        {
            var metadataModels = new List<MetadataModel>();

            foreach (var file in files)
            {
                metadataModels.Add(await _attachService.SaveToTempAsync(file));
            }

            return metadataModels;
        }

        
        [HttpPost]
        public async Task<MetadataModel> UploadFile(IFormFile file)
        {
            return await _attachService.SaveToTempAsync(file);
        }

        [Route("{id}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<FileResult> DisplayAttachById(Guid id)
        {
            var attachModel = await _attachService.GetAttachByIdAsync(id);
            var fileStream = new FileStream(attachModel.FilePath, FileMode.Open);
            return File(fileStream, attachModel.MimeType);
            
           
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<FileResult> DownloadAttachById(Guid id)
        {
            var attachModel = await _attachService.GetAttachByIdAsync(id);

            var fileStream = new FileStream(attachModel.FilePath, FileMode.Open);
            return File(fileStream, attachModel.MimeType, fileDownloadName: attachModel.Name);
        }

    }
}
