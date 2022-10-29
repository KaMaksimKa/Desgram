using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublicationController:ControllerBase
    {
        private readonly IPublicationService _publicationService;
        private readonly IUserService _userService;

        public PublicationController(IPublicationService publicationService,IUserService userService)
        {
            _publicationService = publicationService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        public async Task CreatePublication([FromForm] CreatePublicationModel model)
        {
            var user = await _userService.GetUserByClaimsPrincipalAsync(User);
            await _publicationService.CreatePublicationAsync(model,user);
            return;
        }

        [HttpGet]
        public async Task<List<PublicationModel>> GetPublications()
        {
            return await _publicationService.GetAllPublicationsAsync();
        }
    }
}
