using Desgram.Api.Models.Post;
using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface ILikeService
    {
        public Task<AmountLikesModel> AddLikePostAsync(Guid postId, Guid requestorId);
        public Task<AmountLikesModel> DeleteLikePostAsync(Guid postId, Guid requestorId);
        public Task<int> AddLikeCommentAsync(Guid commentId, Guid requestorId);
        public Task<int> DeleteLikeCommentAsync(Guid commentId, Guid requestorId);
        public Task DeleteAllLikesPostFromUserAsync(Guid userId, Guid requestorId);

    }
}
