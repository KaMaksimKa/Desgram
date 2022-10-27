using Desgram.Api.Models;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserDTO createUserDto);
        public Task<List<UserDTO>> GetUsersAsync();
    }
}
