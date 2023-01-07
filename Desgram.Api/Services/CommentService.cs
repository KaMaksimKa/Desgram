using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Comment;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class CommentService:ICommentService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ICustomMapperService _customMapperService;

        public CommentService(ApplicationContext context,IMapper mapper,INotificationService notificationService, ICustomMapperService customMapperService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
            _customMapperService = customMapperService;
        }

        public async Task<CommentModel> AddCommentAsync(CreateCommentModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            var post = await _context.Posts.GetPostByIdAsync(model.PostId);

            if (!post.IsCommentsEnabled)
            {
                throw new AccessActionException();
            }

            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                User = user,
                Post = post,
                Content = model.Content,
                Likes = new List<LikeComment>(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            var commentEntity = (await _context.Comments.AddAsync(comment)).Entity;
            await _context.SaveChangesAsync();

            if (commentEntity.UserId != post.UserId)
            {
                await _notificationService.CreateCommentNotificationAsync(commentEntity);
            }

            var comments = _context.Comments.AsNoTracking()
                .Where(c => c.Id == comment.Id);

            var commentModel = (await _customMapperService.ToCommentModelsList(comments, requestorId)).FirstOrDefault(); 

            if (commentModel == null)
            {
                throw new CommentNotFoundException();
            }

            return commentModel;
        }

        public async Task DeleteCommentAsync(Guid commentId, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentByIdAsync(commentId);
            var post = await _context.Posts.GetPostByIdAsync(comment.PostId);

            if (comment.UserId != requestorId && post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();

            if (comment.UserId != post.UserId)
            {
                await _notificationService.DeleteCommentNotificationAsync(comment.Id);
            }

         
        }

        public async Task<List<CommentModel>> GetCommentsAsync(CommentRequestModel model, Guid requestorId)
        {
            var comments =  _context.Comments.AsNoTracking()
                .Where(c => c.PostId == model.PostId && c.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take);

            return await _customMapperService.ToCommentModelsList(comments, requestorId);
        }

        public async Task UpdateCommentAsync(UpdateCommentModel model, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentByIdAsync(model.CommentId);

            if (comment.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            comment.Content = model.Content;
            comment.UpdatedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllCommentsFromUserAsync(Guid userId, Guid requestorId)
        {
            var blockingComments = await _context.Posts
                .Where(p => p.UserId == requestorId)
                .SelectMany(p => p.Comments)
                .Where(c => c.UserId == userId && c.DeletedDate == null)
                .ToListAsync();

            foreach (var comment in blockingComments)
            {
                comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            }

            await _context.SaveChangesAsync();

            foreach (var comment in blockingComments)
            {
                await _notificationService.DeleteCommentNotificationAsync(comment.Id);
            }
        }
    }
}
