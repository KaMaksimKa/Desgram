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

        public CommentService(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddCommentAsync(CreateCommentModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            var post = await _context.Posts.GetPostById(model.PostId);

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

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Guid commentId, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentById(commentId);
            var post = await _context.Posts.GetPostById(comment.PostId);

            if (comment.UserId != requestorId && post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentModel>> GetCommentsAsync(CommentRequestModel model, Guid requestorId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == model.PostId && c.DeletedDate == null)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();

            await AfterMapComment(comments, requestorId);

            return comments;
        }

        public async Task UpdateCommentAsync(UpdateCommentModel model, Guid requestorId)
        {
            var comment = await _context.Comments.GetCommentById(model.CommentId);

            if (comment.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            comment.Content = model.Content;
            comment.UpdatedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }
        private async Task AfterMapComment(List<CommentModel> comments, Guid userId)
        {
            var commentsInfo = await _context.Comments
                .Where(c => comments.Select(com => com.Id).Contains(c.Id))
                .Select(c => new {
                    c.Id,
                    IsLiked = c.Likes.Any(l => l.UserId == userId && l.DeletedDate == null)
                }).ToListAsync();

            foreach (var commentModel in comments)
            {
                var commentInfo = commentsInfo.FirstOrDefault(c => c.Id == commentModel.Id);
                if (commentInfo == null)
                {
                    throw new CommentNotFoundException();
                }
                commentModel.IsLiked = commentInfo.IsLiked;
                commentModel.IsAuthor = commentModel.User.Id == userId;
            }
        }

      
    }
}
