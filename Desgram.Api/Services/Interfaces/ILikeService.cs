namespace Desgram.Api.Services.Interfaces
{
    public interface ILikeService
    {
        public Task<int?> AddLikePostAsync(Guid postId, Guid requestorId);
        public Task<int?> DeleteLikePostAsync(Guid postId, Guid requestorId);
        public Task<int> AddLikeCommentAsync(Guid commentId, Guid requestorId);
        public Task<int> DeleteLikeCommentAsync(Guid commentId, Guid requestorId);
    }
}
