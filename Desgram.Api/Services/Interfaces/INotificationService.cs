using Desgram.Api.Models;
using Desgram.Api.Models.Notification;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface INotificationService
    {
        public Task CreateLikePostNotificationAsync(LikePost likePost);
        public Task CreateLikeCommentNotificationAsync(LikeComment likeComment);
        public Task CreateCommentNotificationAsync(Comment comment);
        public Task CreateSubscriptionNotificationAsync(UserSubscription subscription);
        public Task DeleteLikePostNotificationAsync(Guid likePostId);
        public Task DeleteLikeCommentNotificationAsync(Guid likeCommentId);
        public Task DeleteCommentNotificationAsync(Guid commentId);
        public Task DeleteSubscriptionNotificationAsync(Guid subscriptionId);
        public Task<int> GetNumberUnseenNotificationsAsync(Guid requestorId);
        public Task<List<NotificationModel>> GetNotificationsAsync(SkipDateTakeModel model,Guid requestorId);
    }
}
