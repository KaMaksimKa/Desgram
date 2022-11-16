using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Comment;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
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

        public async Task AddCommentAsync(CreateCommentModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);
            var post = await _context.Posts.GetPostById(model.PostId);

            if (!post.IsCommentsEnabled)
            {
                throw new CustomException("comments under this post are disabled");
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

        public async Task DeleteCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await _context.Comments.GetCommentById(commentId);
            var post = await _context.Posts.GetPostById(comment.PostId);

            if (comment.UserId != userId && post.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentModel>> GetCommentsAsync(Guid postId, Guid userId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId && c.DeletedDate == null)
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();

            AfterMapComment(comments, userId);

            return comments;
        }

        public async Task UpdateCommentAsync(UpdateCommentModel model, Guid userId)
        {
            var comment = await _context.Comments.GetCommentById(model.CommentId);

            if (comment.UserId != userId)
            {
                throw new CustomException("you don't have enough right");
            }

            comment.Content = model.Content;
            comment.UpdatedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }
        private void AfterMapComment(List<CommentModel> comment, Guid userId)
        {
            SetIsLikedComment(comment, userId);
        }

        private void SetIsLikedComment(List<CommentModel> comments, Guid userId)
        {
            var commentsLikes = _context.Comments
                .Where(c => comments.Select(com => com.Id).Contains(c.Id)
                            && c.Likes.Any(l => l.UserId == userId && l.DeletedDate == null))
                .Select(p => p.Id);
            
            foreach (var commentModel in comments.Where(p => commentsLikes.Contains(p.Id)))
            {
                commentModel.IsLiked = true;
            }
        }
    }
}
