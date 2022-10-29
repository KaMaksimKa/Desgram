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
        public async Task<UserModel> GetUser()
        {
            var userModel = _mapper.Map<UserModel>(await _userService.GetUserByClaimsPrincipalAsync(User));
            return userModel;
        }

        [HttpGet]
        public async Task<List<UserModel>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return users.Select(u => _mapper.Map<UserModel>(u)).ToList();
        }

        [HttpPost]
        public string UploadImage(IFormFile file)
        {
            return _imageService.SaveImage(file);
        }
    }
}
