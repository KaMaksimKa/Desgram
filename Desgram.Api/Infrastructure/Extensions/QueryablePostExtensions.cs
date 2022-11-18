using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryablePostExtensions
    {
        public static async Task<Post> GetPostById(this IQueryable<Post> posts,Guid id)
        {
            var post = await posts.FirstOrDefaultAsync(p => p.Id == id && p.DeletedDate == null);
            if (post == null)
            {
                throw new PostNotFoundException();
            }

            return post;
        }

        public static IQueryable<Post> GetAvailablePostsByUserId(this IQueryable<Post> posts, Guid userId)
        {
            return posts.Where(p=>p.DeletedDate == null && (!p.User.IsPrivate || p.UserId == userId 
                                               || p.User.Followers
                                                   .Any(s=>s.DeletedDate == null 
                                                           && s.FollowerId == userId
                                                           && s.IsApproved)));
        }
    }
}
