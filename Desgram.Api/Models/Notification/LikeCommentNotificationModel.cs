using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Notification
{
    public class LikeCommentNotificationModel
    {
        public PartialUserModel User { get; set; } = null!;
        public PartialPostModel Post { get; set; } = null!;
        public string Comment { get; set; } = null!;
    }
}
