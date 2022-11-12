using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
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

        public async Task AddLikePostAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts.GetPostById(postId);

            if (await _context.LikesPosts.IsLikedPost(postId, userId))
            {
                throw new CustomException("you've already like this post");
            }

            var user = await _context.Users.GetUserByIdAsync(userId);

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

        public async Task DeleteLikePostAsync(Guid postId, Guid userId)
        {
            var like = await _context.LikesPosts
                .FirstOrDefaultAsync(l => l.UserId == userId
                                          && l.PostId == postId && l.DeletedDate == null);
            if (like == null)
            {
                throw new CustomException("you've not like this post yet");
            }

            if (like.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task AddLikeCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await _context.Comments.GetCommentById(commentId);

            if (await _context.LikesComments.IsLikedComment(commentId, userId))
            {
                throw new CustomException("you've already like this comment");
            }

            var user = await _context.Users.GetUserByIdAsync(userId);

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

        public async Task DeleteLikeCommentAsync(Guid commentId, Guid userId)
        {
            var like = await _context.LikesComments
                .FirstOrDefaultAsync(l => l.CommentId == commentId
                                          && l.UserId == userId && l.DeletedDate == null);

            if (like == null)
            {
                throw new CustomException("you've not like this comment yet");
            }

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }
    }
}
