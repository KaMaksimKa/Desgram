using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Publication;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublicationController:ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [HttpPost]
        public async Task CreatePublication(CreatePublicationModel model)
        {
            var userId = User.GetUserId();
            await _publicationService.CreatePublicationAsync(model, userId);
        }

        [HttpPost]
        public async Task DeletePublication(Guid publicationId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeletePublication(publicationId,userId);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<PublicationModel>> GetPublications()
        {
            return await _publicationService.GetAllPublicationsAsync();
        }


        [HttpGet]
        public async Task<List<PublicationModel>> GetPublicationsByHashTag(string hashTag)
        {
            return await _publicationService.GetPublicationByHashTagAsync(hashTag);
        }

        [HttpPost]
        public async Task AddComment(CreateCommentModel model)
        {
            var userId = User.GetUserId();
            await _publicationService.AddComment(model, userId);
        }

        [HttpPost]
        public async Task DeleteComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteComment(commentId, userId);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<CommentModel>> GetComments(Guid publicationId)
        {
            return await _publicationService.GetComments(publicationId);
        }

        [HttpPost]
        public async Task AddLikePublication(Guid publicationId)
        {
            var userId = User.GetUserId();
            await _publicationService.AddLikePublication(publicationId,userId);
        }
       

        [HttpPost]
        public async Task DeleteLikePublication(Guid publicationId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteLikePublication(publicationId, userId);
        }


        [HttpPost]
        public async Task AddLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.AddLikeComment(commentId, userId);
        }
  

        [HttpPost]
        public async Task DeleteLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteLikeComment(commentId, userId);
        }

    }
}
