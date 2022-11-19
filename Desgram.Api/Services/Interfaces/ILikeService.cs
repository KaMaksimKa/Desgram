namespace Desgram.Api.Services.Interfaces
{
    public interface ILikeService
    {
        public Task AddLikePostAsync(Guid postId, Guid requestorId);
        public Task DeleteLikePostAsync(Guid postId, Guid requestorId);
        public Task AddLikeCommentAsync(Guid commentId, Guid requestorId);
        public Task DeleteLikeCommentAsync(Guid commentId, Guid requestorId);
    }
}
