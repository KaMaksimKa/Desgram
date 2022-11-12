using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
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
                throw new CustomException("post not found");
            }

            return post;
        }
    }
}
