using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Models.Publication;
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
        private readonly IUrlService _urlService;

        public PublicationService(ApplicationContext context,IAttachService attachService,
            IMapper mapper,IUrlService urlService)
        {
            _context = context;
            _attachService = attachService;
            _mapper = mapper;
            _urlService = urlService;
        }
        
        public async Task CreatePublicationAsync(CreatePublicationModel model, Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            var hashTagsString = model.Description.Split().Where(w => w.StartsWith("#")).ToList();
            var hashTags = await GetHashTags(hashTagsString);

            var publication = new Publication()
            {
                Id = Guid.NewGuid(),
                User = user,
                Description = model.Description,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                Comments = new List<Comment>(),
                LikesPublication = new List<LikePublication>(),
                AttachesPublication = model.MetadataModels
                    .Select(meta => new AttachPublication()
                    {
                        Id = Guid.NewGuid(),
                        MimeType = meta.MimeType,
                        CreatedDate = DateTimeOffset.Now.UtcDateTime,
                        Name = meta.Name,
                        Path = _attachService.MoveFromTempToAttach(meta),
                        Owner = user,
                        Size = meta.Size,
                    }).ToList(),
                HashTags = hashTags,
                DeletedDate = null
            };

            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublication(Guid publicationId, Guid userId)
        {
            if (await _context.Publications
                    .FirstOrDefaultAsync(c => c.Id == publicationId && c.DeletedDate == null) is not { } publication)
            {
                throw new CustomException("publication not found");
            }
            
            
            if (publication.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }
            
            publication.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        private async Task<List<HashTag>> GetHashTags(List<string> hashTagsString)
        {
            var hashTagsDb = await _context.HashTags.ToListAsync();

            var hashTags = new List<HashTag>();

            foreach (var hashTagString in hashTagsString)
            {
                if (hashTagsDb.FirstOrDefault(h => h.Title == hashTagString) is { } hashTag)
                {
                    hashTags.Add(hashTag);
                }
                else
                {
                    var newHashTag = (await _context.HashTags.AddAsync(new HashTag()
                    {
                        Id = Guid.NewGuid(),
                        Title = hashTagString
                    })).Entity;

                    hashTags.Add(newHashTag);
                }

            }
            return hashTags;
        }

        public async Task<List<PublicationModel>> GetAllPublicationsAsync()
        {
            var publications =await _context.Publications
                .Where(p => p.DeletedDate == null)
                .ProjectTo<PublicationModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var publication in publications)
            {
                foreach (var contentModel in publication.AttachesPublication)
                {
                    contentModel.Url = _urlService.GetUrlDisplayAttachById(contentModel.Id);
                }
            }

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
                Content = model.Content,
                LikesComment = new List<LikeComment>(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            await _context.Comments.AddAsync(comment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Guid commentId, Guid userId)
        {
            if ( await _context.Comments
                    .Include(c => c.Publication)
                    .FirstOrDefaultAsync(c => c.Id == commentId && c.DeletedDate == null) is not { } comment)
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

            comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task AddLikePublication(Guid publicationId, Guid userId)
        {
            var publication = await GetPublicationByIdAsync(publicationId);

            if (await _context.LikesPublications
                    .FirstOrDefaultAsync(l => l.PublicationId == publicationId && l.UserId == userId && l.DeletedDate == null) != null)
            {
                throw new CustomException("you've already like this post");
            }

            var user = await GetUserByIdAsync(userId);

            var like = new LikePublication()
            {
                Id = Guid.NewGuid(),
                User = user,
                Publication = publication,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            await _context.LikesPublications.AddAsync(like);
       
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikePublication(Guid publicationId, Guid userId)
        {
            if (await _context.LikesPublications
                    .Include(c => c.Publication)
                    .FirstOrDefaultAsync(l => l.UserId == userId && l.PublicationId == publicationId && l.DeletedDate ==null) is not { } like)
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

            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task AddLikeComment(Guid commentId, Guid userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c=>c.Id == commentId && c.DeletedDate == null);
            if (comment == null)
            {
                throw new CustomException("comment not found");
            }

            if (await _context.LikesComments
                    .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId && l.DeletedDate == null ) != null)
            {
                throw new CustomException("you've already like this comment");
            }

            var user = await GetUserByIdAsync(userId);

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

        public async Task DeleteLikeComment(Guid commentId, Guid userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.DeletedDate == null);
            if (comment == null)
            {
                throw new CustomException("comment not found");
            }

            if (await _context.LikesComments
                    .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId && l.DeletedDate == null) is not {} like)
            {
                throw new CustomException("you've not like this comment yet");
            }

            
            like.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }


        public async Task<List<CommentModel>> GetComments(Guid publicationId)
        {
            var comments = await _context.Comments
                .Where(c => c.PublicationId == publicationId && c.DeletedDate == null)
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();

            return comments;
        }

        public async Task<List<PublicationModel>> GetPublicationByHashTagAsync(string hashTag)
        {
            var publication = await _context.Publications
                .Where(p =>p.DeletedDate == null && p.HashTags.Any(p => p.Title == hashTag))
                .ProjectTo<PublicationModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return publication.Select(p => _mapper.Map<PublicationModel>(p)).ToList();
        }

        private async Task<Publication> GetPublicationByIdAsync(Guid id)
        {
            var publication =await _context.Publications.FirstOrDefaultAsync(p => p.Id == id && p.DeletedDate == null);
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
