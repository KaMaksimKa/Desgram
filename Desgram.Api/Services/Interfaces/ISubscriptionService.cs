using Desgram.Api.Models.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(Guid followerId,string contentMakerName);
        public Task UnsubscribeAsync(Guid followerId, string contentMakerName);
        public Task SendSubscriptionRequest(Guid followerId, string contentMakerName);
        public Task DeleteSubscriptionRequest(Guid followerId, string contentMakerName);
        public Task RespondSubscriptionRequest(bool response,Guid requestId,Guid userId);
        public Task<List<FollowingModel>> GetFollowingAsync(Guid userId);
        public Task<List<FollowerModel>> GetFollowersAsync(Guid userId);
    }
}
