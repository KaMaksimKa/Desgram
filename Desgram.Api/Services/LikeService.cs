using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Post;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Desgram.Api.Services
{
    public class LikeService:ILikeService
    {
        private readonly ApplicationContext _context;
        private readonly INotificationService _notificationService;

        public LikeService(ApplicationContext context,INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<AmountLikesModel> AddLikePostAsync(Guid postId, Guid requestorId)
        {
            var post = await _context.Posts.GetPostByIdAsync(postId);

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

            var likePost = (await _context.LikesPosts.AddAsync(like)).Entity;

            await _context.SaveChangesAsync();

            if (like.UserId != post.UserId)
            {
                await _notificationService.CreateLikePostNotificationAsync(likePost);
            }
            

            return new AmountLikesModel()
            {
                AmountLikes = !post.IsLikesVisible ? null :
                    await _context.LikesPosts.Where(l => l.DeletedDate == null && l.PostId == postId).CountAsync()
            };
        }

        public async Task<AmountLikesModel> DeleteLikePostAsync(Guid postId, Guid requestorId)
        {
            var post = await _context.Posts.GetPostByIdAsync(postId);
            var like = await GetLikePostAsync(postId, requestorId);

            if (like.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();

            if (like.UserId != post.UserId)
            {
                await _notificationService.DeleteLikePostNotificationAsync(like.Id);
            }
            

            return new AmountLikesModel()
            {
                AmountLikes = !post.IsLikesVisible ? null :
                    await _context.LikesPosts.Where(l => l.DeletedDate == null && l.PostId == postId).CountAsync()
            };
        }

        public async Task<int> AddLikeCommentAsync(Guid commentId, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentByIdAsync(commentId);

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

            var likeComment = (await _context.LikesComments.AddAsync(like)).Entity;

            await _context.SaveChangesAsync();

            if (like.UserId != comment.UserId)
            {
                await _notificationService.CreateLikeCommentNotificationAsync(likeComment);
            }
           

            return await _context.LikesComments
                .Where(l => l.DeletedDate == null && l.CommentId == commentId)
                .CountAsync();
        }

        public async Task<int> DeleteLikeCommentAsync(Guid commentId, Guid requestorId)
        {
            var like = await GetLikeCommentAsync(commentId, requestorId);
            var comment = await _context.Comments.GetCommentByIdAsync(commentId);

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();

            if (like.UserId != comment.UserId)
            {
                await _notificationService.DeleteLikeCommentNotificationAsync(like.Id);
            }
            
            return await _context.LikesComments
                .Where(l => l.DeletedDate == null && l.CommentId == commentId)
                .CountAsync();
        }

        public async Task DeleteAllLikesPostFromUserAsync(Guid userId, Guid requestorId)
        {
            var blockingLikesPosts = await _context.Posts
                .Where(p => p.UserId == requestorId)
                .SelectMany(p => p.Likes)
                .Where(l => l.UserId == userId && l.DeletedDate == null)
                .ToListAsync();

            foreach (var like in blockingLikesPosts)
            {
                like.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            }

            await _context.SaveChangesAsync();

            foreach (var like in blockingLikesPosts)
            {
                await _notificationService.DeleteLikePostNotificationAsync(like.Id);
            }
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
