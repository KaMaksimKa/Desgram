using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.Subscription;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Api")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Route("{contentMakerId}")]
        [HttpPost]
        public async Task Subscribe(Guid contentMakerId)
        {
            await _subscriptionService.SubscribeAsync(contentMakerId,User.GetUserId());
        }

        [Route("{contentMakerId}")]
        [HttpPost]
        public async Task Unsubscribe(Guid contentMakerId)
        {
            await _subscriptionService.UnsubscribeAsync(contentMakerId,User.GetUserId());
        }

        [Route("{followerId}")]
        [HttpPost]
        public async Task DeleteFollower(Guid followerId)
        {
            await _subscriptionService.DeleteFollowerAsync(followerId,User.GetUserId());
        }

        [Route("{followerId}")]
        [HttpPost]
        public async Task AcceptSubscription(Guid followerId)
        {
            await _subscriptionService.AcceptSubscriptionAsync(followerId,User.GetUserId());
        }

        /*[HttpGet]
        public Task<List<PartialUserModel>> GetFollowing([FromQuery]SkipTakeModel model)
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetUserFollowingAsync(new UserFollowingRequestModel()
            {
                Skip = model.Skip,
                UserId = userId,
                Take = model.Take,
            },userId);
        }*/

        [HttpGet]
        public Task<List<PartialUserModel>> GetUserFollowing([FromQuery]UserFollowingRequestModel model)
        {
            return _subscriptionService.GetUserFollowingAsync(model, User.GetUserId());
        }

        /*[HttpGet]
        public Task<List<PartialUserModel>> GetFollowers([FromQuery]SkipTakeModel model)
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetUserFollowersAsync(new UserFollowersRequestModel()
            {
                Skip = model.Skip,
                UserId = userId,
                Take = model.Take,
            }, userId);
        }*/

        [HttpGet]
        public Task<List<PartialUserModel>> GetUserFollowers([FromQuery]UserFollowersRequestModel model)
        {
            return _subscriptionService.GetUserFollowersAsync(model, User.GetUserId());
        }

        [HttpGet]
        public Task<List<PartialUserModel>> GetSubRequests([FromQuery]SkipTakeModel model) => 
            _subscriptionService.GetSubRequestsAsync(model,User.GetUserId());
        
    }
}
