using Desgram.Api.Models.Comment;

namespace Desgram.Api.Services.Interfaces
{
    public interface ICommentService
    {
        public Task AddCommentAsync(CreateCommentModel model, Guid userId);
        public Task DeleteCommentAsync(Guid commentId, Guid userId);
        public Task<List<CommentModel>> GetCommentsAsync(Guid publicationId, Guid userId);
        public Task UpdateCommentAsync(UpdateCommentModel model, Guid userId);
    }
}
