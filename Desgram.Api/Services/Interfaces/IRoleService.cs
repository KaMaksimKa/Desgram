using Desgram.Api.Models.Role;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IRoleService
    {
        public Task CreateRoleAsync(string name);
        public Task DeleteRoleAsync(string name);
        public Task AddUserToRoleAsync(Guid userId, string roleName);
        public Task DeleteUserFromRoleAsync(Guid userId, string roleName);
        public Task<bool> IsUserInRoleAsync(Guid userId, string roleName);
        public Task<List<ApplicationRoleModel>> GetUserRoles(Guid userId);

    }
}
