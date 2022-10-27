using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileService _imageService;

        public UserController(IUserService userService,IFileService imageService)
        {
            _userService = userService;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDTO createUserDto)
        {
            await _userService.CreateUserAsync(createUserDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok( await _userService.GetUsersAsync());
        }


        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            return Ok(_imageService.SaveImage(file));
        }
    }
}
