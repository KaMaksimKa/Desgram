using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Subscription;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task Subscribe(string subscriptionUserName)
        {
            var userId = User.GetUserId();
            await _subscriptionService.Subscribe(userId, subscriptionUserName);
        }

        [HttpPost]
        public async Task Unsubscribe(string subscriptionUserName)
        {
            var userId = User.GetUserId();
            await _subscriptionService.Unsubscribe(userId, subscriptionUserName);
        }

        [HttpGet]
        public Task<List<SubscriptionModel>> GetSubscriptions()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetSubscriptions(userId);
        }

        [HttpGet]
        public Task<List<SubscriberModel>> GetSubscribers()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetSubscribers(userId);
        }
    }
}
