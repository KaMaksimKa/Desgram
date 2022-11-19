using Desgram.Api.Models.Blocked;
using Desgram.Api.Models.User;

namespace Desgram.Api.Services.Interfaces
{
    public interface IBlockingService
    {
        public Task BlockUserAsync(Guid userId, Guid requestorId);
        public Task UnblockUserAsync(Guid userId, Guid requestorId);
        public Task<List<PartialUserModel>> GetBlockedUsersAsync(Guid requestorId);
    }
}
