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
                Comments = new List<Comment>(),
                LikesPublication = new List<LikePublication>(),
                ImagesPublication = model.MetadataModels
                    .Select(meta => new ImagePublication()
                    {
                        Id = Guid.NewGuid(),
                        MimeType = meta.MimeType,
                        CreatedDate = DateTime.Now,
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
                    UserName = p.User.Name
                }).ToList();
            return publications;
        }

        public async Task AddComment(CreateCommentModel model, Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                User = user,
                Publication = await GetPublicationByIdAsync(model.PublicationId),
                AmountLikes = 0,
                Content = model.Content,
                LikesComment = new List<LikeComment>(),
            };
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

            if (comment.UserId != userId && comment.Publication.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task AddLike(Guid publicationId, Guid userId)
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
            await _context.LikesPublications.AddAsync(like);
            publication.AmountLikes += 1;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLike(Guid publicationId, Guid userId)
        {
            throw new NotImplementedException();
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
            var comments = await _context.Comments.Where(c=>c.PublicationId == publicationId).ToListAsync();

            return comments.Select(c=>_mapper.Map<CommentModel>(c)).ToList();
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
