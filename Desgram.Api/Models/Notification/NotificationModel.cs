using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Notification
{
    public class NotificationModel
    {
        public DateTimeOffset CreatedDate { get; set; }
        public bool HasViewed { get; set; }
        public LikePostNotificationModel? LikePost { get; set; }
        public LikeCommentNotificationModel? LikeComment { get; set; }
        public CommentNotificationModel? Comment { get; set; }
        public SubscriptionNotificationModel? Subscription { get; set; }
    }
}
