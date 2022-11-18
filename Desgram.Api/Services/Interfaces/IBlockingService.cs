using Desgram.Api.Models.Blocked;
using Desgram.Api.Models.User;

namespace Desgram.Api.Services.Interfaces
{
    public interface IBlockingService
    {
        public Task BlockUserAsync(Guid blockUserId, Guid userId);
        public Task UnblockUserAsync(Guid blockUserId, Guid userId);
        public Task<List<PartialUserModel>> GetBlockedUsersAsync(Guid userId);
    }
}
