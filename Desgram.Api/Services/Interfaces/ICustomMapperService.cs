using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Comment;
using Desgram.Api.Models.Notification;
using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;
using Desgram.Api.Services.ServiceModel;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface ICustomMapperService
    {
        public Task<List<PostModel>> ToPostModelsList(IQueryable<Post> posts, Guid requestorId);

        public Task<List<CommentModel>> ToCommentModelsList(IQueryable<Comment> comments, Guid requestorId);

        public Task<List<UserModel>> ToUserModelsList(IQueryable<User> users, Guid requestorId);

        public Task<List<NotificationModel>> ToNotificationModelsList(IQueryable<Notification> notifications);
    }
}
