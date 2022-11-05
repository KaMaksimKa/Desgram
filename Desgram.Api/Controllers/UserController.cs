using Desgram.Api.Infrastructure;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel createUserDto)
        {
            await _userService.CreateUserAsync(createUserDto);
            return Ok();
        }

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public async Task AddAvatar(MetadataModel model)
        {
            var userId = User.GetUserId();

            await _userService.AddAvatarAsync(model,userId);
        }

        [Authorize]
        [HttpGet]
        public async Task<FileResult> GetAvatar()
        {
            var userId = User.GetUserId();

            var attach =  await _userService.GetAvatarAsync(userId);

            return File(await System.IO.File.ReadAllBytesAsync(attach.FilePath) ,attach.MimeType);
        }

    }
}
