using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<Guid> CreateUser(CreateUserModel createUserDto)
        {
            return await _userService.CreateUserAsync(createUserDto);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task ConfirmEmail(string code, Guid unconfirmedUserId)
        {
            await _userService.ConfirmEmailByCodeAsync(code, unconfirmedUserId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task SendEmailConfirmationCode(Guid unconfirmedUserId)
        {
            await _userService.SendEmailConfirmationCodeAsync(unconfirmedUserId);
        }

        [HttpGet]
        public async Task<UserModel> GetCurrentUser()
        {
            var userId = User.GetUserId();

            return await _userService.GetUserByIdAsync(userId);
        }

        [HttpGet]
        public async Task<List<UserModel>> GetUsers()
        {
            return await _userService.GetUsersAsync();
        }

        [HttpPost]
        public async Task AddAvatar(MetadataModel model)
        {
            var userId = User.GetUserId();

            await _userService.AddAvatarAsync(model,userId);
        }

    }
}
