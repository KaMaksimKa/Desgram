using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryableLikeExtensions
    {
        public static async Task<bool> IsLikedPost(this IQueryable<LikePost> likePosts,
            Guid postId, Guid userId)
        {
            return await likePosts
                .AnyAsync(l => l.PostId == postId
                               && l.UserId == userId && l.DeletedDate == null);
        }

        public static async Task<bool> IsLikedComment(this IQueryable<LikeComment> likeComments,
            Guid commentId, Guid userId)
        {
            return await likeComments
                .AnyAsync(l => l.CommentId == commentId
                               && l.UserId == userId && l.DeletedDate == null);
        }
    }
}
