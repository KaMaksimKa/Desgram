using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Api")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlockingController : ControllerBase
    {
        private readonly IBlockingService _blockingService;

        public BlockingController(IBlockingService blockingService)
        {
            _blockingService = blockingService;
        }

        [HttpPost]
        public async Task BlockUser(Guid blockedUserId)
        {
            await _blockingService.BlockUserAsync(blockedUserId, User.GetUserId());
        }

        [HttpPost]
        public async Task UnblockUser(Guid unblockedUserId)
        {
            await _blockingService.UnblockUserAsync(unblockedUserId, User.GetUserId());
        }

        [HttpGet]
        public async Task<List<PartialUserModel>> GetBlockedUsers([FromQuery] SkipTakeModel model)
        {
            return await _blockingService.GetBlockedUsersAsync(model,User.GetUserId());
        }
    }
}
