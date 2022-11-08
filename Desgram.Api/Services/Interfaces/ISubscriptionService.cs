using Desgram.Api.Models.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task Subscribe(Guid subscriberId,string subscriptionUserName);
        public Task Unsubscribe(Guid subscriberId, string subscriptionUserName);
        public Task<List<SubscriptionModel>> GetSubscriptions(Guid userId);
        public Task<List<SubscriberModel>> GetSubscribers(Guid userId);
    }
}
