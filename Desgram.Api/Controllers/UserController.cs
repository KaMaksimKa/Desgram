using System.Security.Claims;
using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;
using Desgram.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileService _imageService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IFileService imageService,IMapper mapper)
        {
            _userService = userService;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel createUserDto)
        {
            await _userService.CreateUserAsync(createUserDto);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userGuid))
            {
                throw new CustomException("you are not authorize");
            }

            var userModel = _mapper.Map<UserModel>(await _userService.GetUserByIdAsync(userGuid));
            return Ok(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            return Ok(_imageService.SaveImage(file));
        }
    }
}
