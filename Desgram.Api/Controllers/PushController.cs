using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Push;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Api")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PushController : ControllerBase
    {
        private readonly IPushService _pushService;

        public PushController(IPushService pushService)
        {
            _pushService = pushService;
        }

        [HttpPost]
        public async Task SubscribePush(PushTokenModel model)
        {
            await _pushService.SubscribePushAsync(model,User.GetSessionId());
        }

        [HttpPost]
        public async Task UnsubscribePush()
        {
            await _pushService.UnsubscribePushAsync(User.GetSessionId());
        }

    }
}
