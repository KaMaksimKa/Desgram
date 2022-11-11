using Desgram.Api.Models.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(Guid subscriberId,string subscriptionUserName);
        public Task UnsubscribeAsync(Guid subscriberId, string subscriptionUserName);
        public Task<List<SubscriptionModel>> GetSubscriptionsAsync(Guid userId);
        public Task<List<SubscriberModel>> GetSubscribersAsync(Guid userId);
    }
}
