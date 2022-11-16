using Desgram.Api.Models.Post;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPostService
    {
        public Task CreatePostAsync(CreatePostModel model, Guid userId);
        public Task DeletePostAsync(Guid publicationId, Guid userId);
        public Task UpdatePostAsync(UpdatePostModel model, Guid userId);
        public Task ChangeLikesVisibilityAsync(ChangeLikesVisibilityModel model, Guid userId);
        public Task ChangeIsCommentsEnabledAsync(ChangeIsCommentsEnabledModel model, Guid userId);
        public Task<List<PostModel>> GetAllPostsAsync(PostRequestModel model, Guid userId);
        public Task<List<PostModel>> GetPostByHashTagAsync(PostByHashtagRequestModel model,Guid userId);
        public Task<List<PostModel>> GetSubscriptionsFeedAsync(PostRequestModel model,Guid userId);
        public Task<List<PostModel>> GetPostsByUserIdAsync(PostByUserIdRequestModel model, Guid userId);
    }
}
