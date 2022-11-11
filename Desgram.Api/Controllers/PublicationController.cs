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


        [AllowAnonymous]
        [HttpGet]
        public async Task<List<PublicationModel>> GetPublications()
        {
            return await _publicationService.GetAllPublicationsAsync();
        }

        [Route("{hashTag}")]
        [HttpGet]
        public async Task<List<PublicationModel>> GetPublicationsByHashTag(string hashTag)
        {
            return await _publicationService.GetPublicationByHashTagAsync(hashTag);
        }


        [HttpGet]
        public async Task<List<PublicationModel>> GetSubscriptionsFeed(int skip,int take)
        {
            var userId = User.GetUserId();
            return await _publicationService.GetSubscriptionsFeedAsync(userId,skip,take);
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
            await _publicationService.DeletePublicationAsync(publicationId,userId);
        }

        [HttpPost]
        public async Task AddComment(CreateCommentModel model)
        {
            var userId = User.GetUserId();
            await _publicationService.AddCommentAsync(model, userId);
        }

        [HttpPost]
        public async Task DeleteComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteCommentAsync(commentId, userId);
        }

        [Route("{publicationId}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<CommentModel>> GetCommentsByPublication(Guid publicationId)
        {
            return await _publicationService.GetCommentsAsync(publicationId);
        }

        [HttpPost]
        public async Task AddLikePublication(Guid publicationId)
        {
            var userId = User.GetUserId();
            await _publicationService.AddLikePublicationAsync(publicationId,userId);
        }
       

        [HttpPost]
        public async Task DeleteLikePublication(Guid publicationId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteLikePublicationAsync(publicationId, userId);
        }


        [HttpPost]
        public async Task AddLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.AddLikeCommentAsync(commentId, userId);
        }
  

        [HttpPost]
        public async Task DeleteLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _publicationService.DeleteLikeCommentAsync(commentId, userId);
        }

    }
}
