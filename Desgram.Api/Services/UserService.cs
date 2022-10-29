using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
   
        public UserService(IMapper mapper,IUserManager userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task CreateUserAsync(CreateUserModel createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            await _userManager.CreateAsync(user);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = (await _userManager.GetAll()).ToList();
            return users;
        }

        public Task<User> GetUserByClaimsPrincipalAsync(ClaimsPrincipal principal)
        {
            var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userGuid))
            {
                throw new CustomException("you are not authorize");
            }

            return _userManager.GetByIdAsync(userGuid);
        }
    }
}
