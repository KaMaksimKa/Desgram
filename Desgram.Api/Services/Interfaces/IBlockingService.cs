using Desgram.Api.Models.Blocked;

namespace Desgram.Api.Services.Interfaces
{
    public interface IBlockingService
    {
        public Task BlockUserAsync(Guid userId,string blockName);
        public Task UnblockUserAsync(Guid userId, string unblockName);
        public Task<List<BlockedUserModel>> GetBlockedUsersAsync(Guid userId);
    }
}
