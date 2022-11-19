using Desgram.Api.Models.Post;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPostService
    {
        public Task CreatePostAsync(CreatePostModel model, Guid requestorId);
        public Task DeletePostAsync(Guid publicationId, Guid requestorId);
        public Task UpdatePostAsync(UpdatePostModel model, Guid requestorId);
        public Task ChangeLikesVisibilityAsync(ChangeLikesVisibilityModel model, Guid requestorId);
        public Task ChangeIsCommentsEnabledAsync(ChangeIsCommentsEnabledModel model, Guid requestorId);
        public Task<List<PostModel>> GetAllPostsAsync(PostRequestModel model, Guid requestorId);
        public Task<List<PostModel>> GetPostByHashTagAsync(PostByHashtagRequestModel model,Guid requestorId);
        public Task<List<PostModel>> GetSubscriptionsFeedAsync(PostRequestModel model,Guid requestorId);
        public Task<List<PostModel>> GetUserPostsAsync(PostByUserIdRequestModel model, Guid requestorId);
    }
}
