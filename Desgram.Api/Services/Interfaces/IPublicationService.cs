using Desgram.Api.Models;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPublicationService
    {
        public Task CreatePublicationAsync(CreatePublicationModel model,User user);
        public Task<List<PublicationModel>> GetAllPublicationsAsync();
        public Task AddComment(CreateCommentModel model, Guid publicationId, User user);
        public Task DeleteComment(Guid commentId, User user);
        public Task AddLike(Guid publicationId, User user);
        public Task DeleteLike(Guid publicationId, User user);
        public Task AddLikeComment(Guid commentId, User user);
        public Task DeleteLikeComment(Guid commentId, User user);
        public Task GetComments(Guid publicationId);
    }
}
