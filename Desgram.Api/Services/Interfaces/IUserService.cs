using System.Security.Claims;
using Desgram.Api.Models;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserModel createUserDto);
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByClaimsPrincipalAsync(ClaimsPrincipal principal);
    }
}
