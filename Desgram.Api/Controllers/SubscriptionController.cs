using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
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
            var model = new SubscriptionModel()
            {
                FollowerId = User.GetUserId(),
                ContentMakerId = contentMakerId
            };

            await _subscriptionService.SubscribeAsync(model);
        }

        [Route("{contentMakerId}")]
        [HttpPost]
        public async Task Unsubscribe(Guid contentMakerId)
        {
            var model = new SubscriptionModel()
            {
                FollowerId = User.GetUserId(),
                ContentMakerId = contentMakerId
            };
            await _subscriptionService.UnsubscribeAsync(model);
        }

        [Route("{followerId}")]
        [HttpPost]
        public async Task DeleteFollower(Guid followerId)
        {
            var model = new SubscriptionModel()
            {
                FollowerId = followerId,
                ContentMakerId = User.GetUserId()
            };
            await _subscriptionService.DeleteFollowerAsync(model);
        }

        [Route("{followerId}")]
        [HttpPost]
        public async Task AcceptSubscription(Guid followerId)
        {
            var model = new SubscriptionModel()
            {
                FollowerId = followerId,
                ContentMakerId = User.GetUserId()
            };

            await _subscriptionService.AcceptSubscriptionAsync(model);
        }

        [HttpGet]
        public Task<List<PartialUserModel>> GetFollowing()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetFollowingAsync(userId);
        }

        [HttpGet]
        public Task<List<PartialUserModel>> GetFollowers()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetFollowersAsync(userId);
        }

        [HttpGet]
        public Task<List<PartialUserModel>> GetSubRequests() => 
            _subscriptionService.GetSubRequestsAsync(User.GetUserId());
        
    }
}
