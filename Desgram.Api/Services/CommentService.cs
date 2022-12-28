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

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();


            var commentModel = await _context.Comments.AsNoTracking()
                .ProjectToByRequestorId<CommentModel>(_mapper.ConfigurationProvider,requestorId)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            if (commentModel == null)
            {
                throw new CommentNotFoundException();
            }

            return _mapper.Map<CommentModel>(commentModel);
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
        }

        public async Task<List<CommentModel>> GetCommentsAsync(CommentRequestModel model, Guid requestorId)
        {
            var comments = await _context.Comments.AsNoTracking()
                .Where(c => c.PostId == model.PostId && c.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take)
                .ProjectToByRequestorId<CommentModel>(_mapper.ConfigurationProvider,requestorId)
                .ToListAsync();

            return comments.Select(c=>_mapper.Map<CommentModel>(c)).ToList();
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
    }
}
