using Desgram.Api.Models.Publication;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPublicationService
    {
        public Task CreatePublicationAsync(CreatePublicationModel model, Guid userId);
        public Task DeletePublication(Guid publicationId, Guid userId);
        public Task<List<PublicationModel>> GetAllPublicationsAsync();
        public Task AddComment(CreateCommentModel model, Guid userId);
        public Task DeleteComment(Guid commentId, Guid userId);
        public Task AddLikePublication(Guid publicationId, Guid userId);
        public Task DeleteLikePublication(Guid publicationId, Guid userId);
        public Task AddLikeComment(Guid commentId, Guid userId);
        public Task DeleteLikeComment(Guid commentId, Guid userId);
        public Task<List<CommentModel>> GetComments(Guid publicationId);
        public Task<List<PublicationModel>> GetPublicationByHashTagAsync(string hashTag);
    }
}
