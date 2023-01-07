using System.ComponentModel.DataAnnotations;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
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
        public async Task TryCreateUser(TryCreateUserModel createUserDto) =>
            await _userService.TryCreateUserAsync(createUserDto);

        [ApiExplorerSettings(GroupName = "Auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task CreateUser(CreateUserModel model) =>
            await _userService.CreateUserAsync(model);

        [ApiExplorerSettings(GroupName = "Auth")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<GuidIdModel> SendSingUpCode(string email) =>
            await _userService.SendSingUpCodeAsync(email);


        [HttpGet]
        public async Task<UserModel> GetCurrentUser() =>
            await _userService.GetUserByIdAsync(User.GetUserId(), User.GetUserId());

        [HttpGet]
        public GuidIdModel GetCurrentUserId() =>
            new GuidIdModel()
            {
                Id = User.GetUserId()
            };

        [HttpGet]
        public async Task<UserModel> GetUserById(Guid userId) =>
            await _userService.GetUserByIdAsync(userId, User.GetUserId());

        [HttpGet]
        public async Task<PersonalInformationModel> GetPersonalInformation() =>
            await _userService.GetPersonalInformationAsync(User.GetUserId());


        [HttpGet]
        public async Task<List<PartialUserModel>> SearchUsersByName([FromQuery]SearchUsersByNameModel model) => 
            await _userService.SearchUsersByNameAsync(model,User.GetUserId());

        [HttpPost]
        public async Task AddAvatar(MetadataModel model) => 
            await _userService.AddAvatarAsync(model, User.GetUserId());

        [HttpPost]
        public async Task DeleteAvatar() =>
            await _userService.DeleteAvatarAsync(User.GetUserId());

        [HttpPost]
        public async Task UpdateProfile(ProfileModel model) =>
           await  _userService.UpdateProfileAsync(model, User.GetUserId());

        [HttpPost]
        public async Task UpdateBirthday(UpdateBirthdayModel model) =>
            await _userService.UpdateBirthdayAsync(model, User.GetUserId());

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
        public async Task TryChangeEmailAsync(string newEmail) =>
            await _userService.TryChangeEmailAsync(newEmail, User.GetUserId());

        [HttpPost]
        public async Task<GuidIdModel> SendChangeEmailCode(string newEmail) =>
            await _userService.SendChangeEmailCodeAsync(newEmail, User.GetUserId());

        [HttpPost]
        public async Task ChangeEmail(ChangeEmailModel model) =>
            await _userService.ChangeEmailAsync(model, User.GetUserId());

        

    }
}
