namespace Desgram.Api.Services.Interfaces
{
    public interface ILikeService
    {
        public Task AddLikePublicationAsync(Guid publicationId, Guid userId);
        public Task DeleteLikePublicationAsync(Guid publicationId, Guid userId);
        public Task AddLikeCommentAsync(Guid commentId, Guid userId);
        public Task DeleteLikeCommentAsync(Guid commentId, Guid userId);
    }
}
