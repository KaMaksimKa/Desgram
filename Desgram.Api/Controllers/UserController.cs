using System.ComponentModel.DataAnnotations;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Api")]
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

        [ApiExplorerSettings(GroupName = "Auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<Guid> CreateUser(CreateUserModel createUserDto) => 
            await _userService.CreateUserAsync(createUserDto);

        [ApiExplorerSettings(GroupName = "Auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task ConfirmUser(ConfirmUserModel model) => 
            await _userService.ConfirmUserAsync(model);

        [ApiExplorerSettings(GroupName = "Auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task SendSingUpCode(Guid unconfirmedUserId) => 
            await _userService.SendSingUpCodeAsync(unconfirmedUserId);

        [HttpGet]
        public async Task<UserModel> GetCurrentUser() =>
            await _userService.GetUserByIdAsync(User.GetUserId(), User.GetUserId());

        [HttpGet]
        public async Task<UserModel> GetUserById(Guid userId) =>
            await _userService.GetUserByIdAsync(userId, User.GetUserId());


        [HttpGet]
        public async Task<List<PartialUserModel>> SearchUsersByName([FromQuery]SearchUsersByNameModel model) => 
            await _userService.SearchUsersByNameAsync(model,User.GetUserId());

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
        public async Task ChangeAccountAvailability([Required]bool isPrivate) =>
            await _userService.ChangeAccountAvailabilityAsync(isPrivate, User.GetUserId());

        [HttpPost]
        public async Task<Guid> ChangeEmail(ChangeEmailModel model) =>
            await _userService.ChangeEmailAsync(model, User.GetUserId());

        [HttpPost]
        public async Task ConfirmEmail(ConfirmEmailModel model) =>
            await _userService.ConfirmEmailAsync(model, User.GetUserId());

        [HttpPost]
        public async Task SendChangeEmailCode(Guid unconfirmedEmailId) =>
            await _userService.SendChangeEmailCodeAsync(unconfirmedEmailId, User.GetUserId());

    }
}
