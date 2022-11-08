using System.Security.Claims;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserModel createUserDto);
        public Task<List<UserModel>> GetUsersAsync();
        public Task<UserModel> GetUserByIdAsync(Guid userId);
        public Task AddAvatarAsync(MetadataModel model,Guid userId);
        public Task<AttachWithPathModel> GetAvatarAsync(Guid userId);

    }
}
