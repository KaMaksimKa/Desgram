using Desgram.Api.Models.Publication;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPublicationService
    {
        public Task CreatePublicationAsync(CreatePublicationModel model, Guid userId);
        public Task DeletePublicationAsync(Guid publicationId, Guid userId);
        public Task<List<PublicationModel>> GetAllPublicationsAsync();
        public Task AddCommentAsync(CreateCommentModel model, Guid userId);
        public Task DeleteCommentAsync(Guid commentId, Guid userId);
        public Task AddLikePublicationAsync(Guid publicationId, Guid userId);
        public Task DeleteLikePublicationAsync(Guid publicationId, Guid userId);
        public Task AddLikeCommentAsync(Guid commentId, Guid userId);
        public Task DeleteLikeCommentAsync(Guid commentId, Guid userId);
        public Task<List<CommentModel>> GetCommentsAsync(Guid publicationId);
        public Task<List<PublicationModel>> GetPublicationByHashTagAsync(string hashTag);
        public Task<List<PublicationModel>> GetSubscriptionsFeedAsync(Guid userId,int skip,int take);
        public Task UpdatePublicationAsync(UpdatePublicationModel model, Guid userId);
    }
}
