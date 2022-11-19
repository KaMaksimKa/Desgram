using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class LikeService:ILikeService
    {
        private readonly ApplicationContext _context;

        public LikeService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddLikePostAsync(Guid postId, Guid requestorId)
        {
            var post = await _context.Posts.GetPostById(postId);

            if (await CheckLikePost(postId, requestorId))
            {
                throw new LikeAlreadyExistsException();
            }

            var user = await _context.Users.GetUserByIdAsync(requestorId);

            var like = new LikePost()
            {
                Id = Guid.NewGuid(),
                User = user,
                Post = post,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            await _context.LikesPosts.AddAsync(like);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikePostAsync(Guid postId, Guid requestorId)
        {
            var like = await GetLikePostAsync(postId, requestorId);

            if (like.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task AddLikeCommentAsync(Guid commentId, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentById(commentId);

            if (await CheckLikeComment(commentId, requestorId))
            {
                throw new LikeAlreadyExistsException();
            }

            var user = await _context.Users.GetUserByIdAsync(requestorId);

            var like = new LikeComment()
            {
                Id = Guid.NewGuid(),
                User = user,
                Comment = comment,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            await _context.LikesComments.AddAsync(like);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikeCommentAsync(Guid commentId, Guid requestorId)
        {
            var like = await GetLikeCommentAsync(commentId, requestorId);

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        private async Task<LikeComment> GetLikeCommentAsync(Guid commentId, Guid userId)
        {
            var like = await _context.LikesComments
                .FirstOrDefaultAsync(l => l.CommentId == commentId
                                          && l.UserId == userId 
                                          && l.DeletedDate == null);

            if (like == null)
            {
                throw new LikeNotFoundException();
            }

            return like;
        }

        private async Task<LikePost> GetLikePostAsync(Guid postId, Guid userId)
        {
            var like = await _context.LikesPosts
                .FirstOrDefaultAsync(l => l.PostId == postId
                                          && l.UserId == userId
                                          && l.DeletedDate == null);

            if (like == null)
            {
                throw new LikeNotFoundException();
            }

            return like;
        }

        private async Task<bool> CheckLikePost(Guid postId, Guid userId)
        {
            return await _context.LikesPosts
                .AnyAsync(l => l.PostId == postId
                               && l.UserId == userId && l.DeletedDate == null);
        }

        private async Task<bool> CheckLikeComment(Guid commentId, Guid userId)
        {
            return await _context.LikesComments
                .AnyAsync(l => l.CommentId == commentId
                               && l.UserId == userId && l.DeletedDate == null);
        }
    }
}
