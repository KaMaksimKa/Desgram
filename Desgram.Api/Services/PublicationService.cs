using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class PublicationService:IPublicationService
    {
        private readonly ApplicationContext _context;
        private readonly IAttachService _attachService;
        private readonly IMapper _mapper;

        public PublicationService(ApplicationContext context,IAttachService attachService,IMapper mapper)
        {
            _context = context;
            _attachService = attachService;
            _mapper = mapper;
        }
        
        public async Task CreatePublicationAsync(CreatePublicationModel model, Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            var publication = new Publication()
            {
                Id = Guid.NewGuid(),
                User = user,
                Description = model.Description,
                AmountLikes = 0,
                AmountComments = 0,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                Comments = new List<Comment>(),
                LikesPublication = new List<LikePublication>(),
                ImagesPublication = model.MetadataModels
                    .Select(meta => new ImagePublication()
                    {
                        Id = Guid.NewGuid(),
                        MimeType = meta.MimeType,
                        CreatedDate = DateTimeOffset.Now.UtcDateTime,
                        Name = meta.Name,
                        Path = _attachService.MoveFromTempToAttach(meta),
                        Owner = user
                    }).ToList()
            };

            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PublicationModel>> GetAllPublicationsAsync()
        {
            var publications = (await _context.Publications
                    .Include(p=>p.ImagesPublication)
                    .Include(p=>p.User).ToListAsync())
                .Select(p=> new PublicationModel()
                {
                    Id = p.Id,
                    AmountLikes = p.AmountLikes,
                    Description = p.Description,
                    ImageGuidList = p.ImagesPublication.Select(i=>i.Id).ToList(),
                    UserName = p.User.Name,
                    AmountComments = p.AmountComments
                }).ToList();
            return publications;
        }

        public async Task AddComment(CreateCommentModel model, Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            var publication = await GetPublicationByIdAsync(model.PublicationId);

            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                User = user,
                Publication = publication,
                AmountLikes = 0,
                Content = model.Content,
                LikesComment = new List<LikeComment>(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime
            };

            publication.AmountComments += 1;
            await _context.Comments.AddAsync(comment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Guid commentId, Guid userId)
        {
            if ( await _context.Comments
                    .Include(c => c.Publication)
                    .FirstOrDefaultAsync(c => c.Id == commentId) is not { } comment)
            {
                throw new CustomException("comment not found");
            }

            if (comment.Publication == null)
            {
                throw new CustomException("forgot include Publication");
            }

            if (comment.UserId != userId && comment.Publication.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            comment.Publication.AmountComments -= 1;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task AddLikePublication(Guid publicationId, Guid userId)
        {
            var publication = await GetPublicationByIdAsync(publicationId);

            if (await _context.LikesPublications
                    .FirstOrDefaultAsync(l => l.PublicationId == publicationId && l.UserId == userId) != null)
            {
                throw new CustomException("you've already like this post");
            }

            var user = await GetUserByIdAsync(userId);

            var like = new LikePublication()
            {
                Id = Guid.NewGuid(),
                User = user,
                Publication = publication,
            };

            publication.AmountLikes += 1;
            await _context.LikesPublications.AddAsync(like);
       
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikePublication(Guid publicationId, Guid userId)
        {
            if (await _context.LikesPublications
                    .Include(c => c.Publication)
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.PublicationId == publicationId) is not { } like)
            {
                throw new CustomException("you've not like this post yet");
            }

            if (like.Publication == null)
            {
                throw new CustomException("forgot include Publication");
            }

            if (like.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            like.Publication.AmountComments -= 1;
            _context.LikesPublications.Remove(like);

            await _context.SaveChangesAsync();
        }

        public async Task AddLikeComment(Guid commentId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLikeComment(Guid commentId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CommentModel>> GetComments(Guid publicationId)
        {
            var comments = await _context.Comments
                .Include(c=>c.User)
                .Where(c=>c.PublicationId == publicationId)
                .ToListAsync();

            return comments.Select(c=> new CommentModel()
            {
                AmountLikes = c.AmountLikes,
                Content = c.Content,
                Id = c.Id,
                UserName = c.User.Name
            }).ToList();
        }


        private async Task<Publication> GetPublicationByIdAsync(Guid id)
        {
            var publication =await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);
            if (publication == null)
            {
                throw new CustomException("publication not found");
            }

            return publication;
        }

        private async Task<User> GetUserByIdAsync(Guid id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }
    }
}
