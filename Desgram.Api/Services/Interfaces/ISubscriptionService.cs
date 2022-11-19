using Desgram.Api.Models.User;
using Desgram.Api.Services.ServiceModel.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(Guid contentMakerId, Guid requestorId);
        public Task UnsubscribeAsync(Guid contentMakerId, Guid requestorId);
        public Task DeleteFollowerAsync(Guid followerId, Guid requestorId);
        public Task AcceptSubscriptionAsync(Guid followerId, Guid requestorId);
        public Task<List<PartialUserModel>> GetSubRequestsAsync(Guid requestorId);
        public Task<List<PartialUserModel>> GetUserFollowingAsync(Guid  userId,Guid requestorId);
        public Task<List<PartialUserModel>> GetUserFollowersAsync(Guid userId,Guid requestorId);

    }
}
