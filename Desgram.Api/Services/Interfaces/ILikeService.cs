namespace Desgram.Api.Services.Interfaces
{
    public interface ILikeService
    {
        public Task AddLikePostAsync(Guid postId, Guid userId);
        public Task DeleteLikePostAsync(Guid postId, Guid userId);
        public Task AddLikeCommentAsync(Guid commentId, Guid userId);
        public Task DeleteLikeCommentAsync(Guid commentId, Guid userId);
    }
}
