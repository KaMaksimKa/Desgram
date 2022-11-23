using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryableCommentExtensions
    {
        public static async Task<Comment> GetCommentByIdAsync(this IQueryable<Comment> comments,Guid commentId)
        {
            var comment = await comments.FirstOrDefaultAsync(c => c.Id == commentId && c.DeletedDate == null);
            if (comment == null)
            {
                throw new CommentNotFoundException();
            }

            return comment;
        }
    }
}
