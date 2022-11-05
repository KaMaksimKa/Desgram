using Desgram.Api.Infrastructure;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [Authorize]

        [HttpPost]
        public async Task Subscribe(string subscriptionUserName)
        {
            var userId = User.GetUserId();
            await _subscriptionService.Subscribe(userId, subscriptionUserName);
        }

        [Authorize]
        [HttpPost]
        public async Task Unsubscribe(string subscriptionUserName)
        {
            var userId = User.GetUserId();
            await _subscriptionService.Unsubscribe(userId, subscriptionUserName);
        }

        [Authorize]
        [HttpGet]
        public Task<List<SubscriptionModel>> GetSubscriptions()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetSubscriptions(userId);
        }

        [Authorize]
        [HttpGet]
        public Task<List<SubscriptionModel>> GetSubscribers()
        {
            var userId = User.GetUserId();
            return _subscriptionService.GetSubscribers(userId);
        }
    }
}
