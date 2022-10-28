using Desgram.Api.Models;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserModel createUserDto);
        public Task<List<UserModel>> GetUsersAsync();
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserNameAsync(string name);
        public Task<User> GetUserByIdAsync(Guid id);
    }
}
