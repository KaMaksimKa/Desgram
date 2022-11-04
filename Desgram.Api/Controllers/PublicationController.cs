using Desgram.Api.Infrastructure;
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

        public PublicationController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [Authorize]
        [HttpPost]
        public async Task CreatePublication([FromForm] CreatePublicationModel model)
        {
            var userId = UserHelper.GetUserIdByClaimsPrincipal(User);
            await _publicationService.CreatePublicationAsync(model, userId);
        }
        [HttpGet]
        public async Task<List<PublicationModel>> GetPublications()
        {
            return await _publicationService.GetAllPublicationsAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task AddComment(CreateCommentModel model)
        {
            var userId = UserHelper.GetUserIdByClaimsPrincipal(User);
            await _publicationService.AddComment(model, userId);
        }
        [Authorize]
        [HttpPost]
        public async Task DeleteComment(DeleteCommentModel model)
        {
            var userId = UserHelper.GetUserIdByClaimsPrincipal(User);
            await _publicationService.DeleteComment(model.CommentId, userId);
        }
        [HttpGet]
        public async Task<List<CommentModel>> GetComments([FromForm]GetCommentsModel model)
        {
            return await _publicationService.GetComments(model.PublicationId);
        }

    }
}
