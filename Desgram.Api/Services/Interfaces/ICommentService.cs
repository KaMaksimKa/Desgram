using Desgram.Api.Models.Publication;

namespace Desgram.Api.Services.Interfaces
{
    public interface ICommentService
    {
        public Task AddCommentAsync(CreateCommentModel model, Guid userId);
        public Task DeleteCommentAsync(Guid commentId, Guid userId);
        public Task<List<CommentModel>> GetCommentsAsync(Guid publicationId);
        public Task UpdateCommentAsync(UpdateCommentModel model, Guid userId);
    }
}
