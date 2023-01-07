using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Notification
{
    public class SubscriptionNotificationModel
    {
        public PartialUserModel User { get; set; } = null!;
        public bool IsApproved { get; set; }
    }
}
