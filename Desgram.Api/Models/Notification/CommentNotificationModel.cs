using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Notification
{
    public class CommentNotificationModel
    {
        public PartialUserModel User { get; set; } = null!;
        public PartialPostModel Post { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
