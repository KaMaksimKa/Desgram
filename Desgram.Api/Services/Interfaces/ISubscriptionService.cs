using Desgram.Api.Models;
using Desgram.Api.Models.Subscription;
using Desgram.Api.Models.User;
using Desgram.Api.Services.ServiceModel.Subscription;

namespace Desgram.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task SubscribeAsync(Guid contentMakerId, Guid requestorId);
        public Task UnsubscribeAsync(Guid contentMakerId, Guid requestorId);
        /// <summary>
        /// Удалить своего подписчика
        /// </summary>
        /// <param name="followerId"></param>
        /// <param name="requestorId"></param>
        /// <returns></returns>
        public Task DeleteFollowerAsync(Guid followerId, Guid requestorId);
        /// <summary>
        /// Принять запрос на подписку
        /// </summary>
        /// <param name="followerId"></param>
        /// <param name="requestorId"></param>
        /// <returns></returns>
        public Task AcceptSubscriptionAsync(Guid followerId, Guid requestorId);
        /// <summary>
        /// Получить все запросы на подписку
        /// </summary>
        /// <param name="model"></param>
        /// <param name="requestorId"></param>
        /// <returns></returns>
        public Task<List<PartialUserModel>> GetSubRequestsAsync(SkipTakeModel model,Guid requestorId);
        public Task<List<PartialUserModel>> GetUserFollowingAsync(UserFollowingRequestModel model,Guid requestorId);
        public Task<List<PartialUserModel>> GetUserFollowersAsync(UserFollowersRequestModel model, Guid requestorId);

    }
}
