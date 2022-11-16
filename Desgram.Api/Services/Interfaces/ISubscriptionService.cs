using Desgram.Api.Models.User;
using Desgram.Api.Services.ServiceModel.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(SubscriptionModel model);
        public Task UnsubscribeAsync(SubscriptionModel model);
        public Task DeleteFollowerAsync(SubscriptionModel model);
        public Task AcceptSubscriptionAsync(SubscriptionModel model);
        public Task<List<PartialUserModel>> GetFollowingAsync(Guid userId);
        public Task<List<PartialUserModel>> GetFollowersAsync(Guid userId);
        public Task<List<PartialUserModel>> GetSubRequestsAsync(Guid userId);
    }
}
