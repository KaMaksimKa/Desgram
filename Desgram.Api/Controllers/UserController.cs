using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.Api.Services;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL.Entities;
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
        public async Task<Guid> CreateUser(CreateUserModel createUserDto) => 
            await _userService.CreateUserAsync(createUserDto);

        [AllowAnonymous]
        [HttpPost]
        public async Task ConfirmUser(string code, Guid unconfirmedUserId) => 
            await _userService.ConfirmUserAsync(code, unconfirmedUserId);

        [AllowAnonymous]
        [HttpPost]
        public async Task SendSingUpCode(Guid unconfirmedUserId) => 
            await _userService.SendSingUpCodeAsync(unconfirmedUserId);

        [HttpGet]
        public async Task<UserModel> GetCurrentUser() =>
            await _userService.GetUserByIdAsync(User.GetUserId());

        [HttpGet]
        public async Task<List<UserModel>> GetUsers() => await _userService.GetUsersAsync();

        [HttpPost]
        public async Task AddAvatar(MetadataModel model) => 
            await _userService.AddAvatarAsync(model, User.GetUserId());

        [HttpPost]
        public async Task UpdateProfile(ProfileModel model) =>
           await  _userService.UpdateProfileAsync(model, User.GetUserId());

        [HttpPost]
        public async Task ChangeUserName(ChangeUserNameModel model) => 
            await _userService.ChangeUserNameAsync(model, User.GetUserId());

        [HttpPost]
        public  async Task ChangePasswordAsync(ChangePasswordModel model) => 
            await _userService.ChangePasswordAsync(model, User.GetUserId());

        [HttpPost]
        public async Task<Guid> ChangeEmail(ChangeEmailModel model) =>
            await _userService.ChangeEmailAsync(model, User.GetUserId());

        [HttpPost]
        public async Task ConfirmEmail(Guid unconfirmedEmailId,string code) =>
            await _userService.ConfirmEmailAsync(code,unconfirmedEmailId, User.GetUserId());

        [HttpPost]
        public async Task SendChangeEmailCode(Guid unconfirmedEmailId) =>
            await _userService.SendChangeEmailCodeAsync(unconfirmedEmailId, User.GetUserId());

    }
}
