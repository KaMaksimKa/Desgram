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

        public async Task DeletePublicationAsync(Guid publicationId, Guid userId)
        {
            var publication = await GetPublicationById(publicationId);

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
            var publications =(await _context.Publications
                .Where(p => p.DeletedDate == null)
                .ProjectTo<PublicationModel>(_mapper.ConfigurationProvider)
                .ToListAsync());

            SetUrlForAttachPublication(publications);

            return publications;
        }

        public async Task AddCommentAsync(CreateCommentModel model, Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            var publication = await GetPublicationById(model.PublicationId);

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

        public async Task DeleteCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await GetCommentById(commentId);
            var publication = await GetPublicationById(comment.PublicationId);

            if (comment.UserId != userId && publication.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            comment.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task AddLikePublicationAsync(Guid publicationId, Guid userId)
        {
            var publication = await GetPublicationById(publicationId);

            if (await UserLikePublicationAsync(publicationId,userId))
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

        public async Task DeleteLikePublicationAsync(Guid publicationId, Guid userId)
        {
            var like = await _context.LikesPublications
                .FirstOrDefaultAsync(l => l.UserId == userId
                                          && l.PublicationId == publicationId && l.DeletedDate == null);
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
            var comment = await GetCommentById(commentId);

            if (await UserLikeCommentAsync(commentId,userId))
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

        public async Task<List<CommentModel>> GetCommentsAsync(Guid publicationId)
        {
            var comments = await _context.Comments
                .Where(c => c.PublicationId == publicationId && c.DeletedDate == null)
                .ProjectTo<CommentModel>(_mapper.ConfigurationProvider).ToListAsync();

            return comments;
        }

        public async Task<List<PublicationModel>> GetPublicationByHashTagAsync(string hashTag)
        {
            var publications = await _context.Publications
                .Where(p =>p.DeletedDate == null && p.HashTags.Any(tag => tag.Title == hashTag))
                .ProjectTo<PublicationModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            SetUrlForAttachPublication(publications);
            return publications;
        }

        public async Task<List<PublicationModel>> GetSubscriptionsFeedAsync(Guid userId, int skip, int take)
        {

            var subscriptionIds = _context.UserSubscriptions
                .Where(s => s.SubscriberId == userId && s.DeletedDate == null)
                .Select(s => s.SubscriptionId);

            var publications = await _context.Publications
                .Where(p => subscriptionIds.Contains(p.UserId) && p.DeletedDate == null)
                .ProjectTo<PublicationModel>(_mapper.ConfigurationProvider)
                .Skip(skip).Take(take)
                .ToListAsync();

            SetUrlForAttachPublication(publications);

            return publications;
        }

        public async Task UpdatePublicationAsync(UpdatePublicationModel model, Guid userId)
        {
            throw new NotImplementedException();
        }

        private void SetUrlForAttachPublication(List<PublicationModel> publications)
        {
            foreach (var publication in publications)
            {
                foreach (var contentModel in publication.AttachesPublication)
                {
                    contentModel.Url = _urlService.GetUrlDisplayAttachById(contentModel.Id);
                }
            }
        }

        private async Task<Publication> GetPublicationById(Guid id)
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

        private async Task<Comment> GetCommentById(Guid commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.DeletedDate == null);
            if (comment == null)
            {
                throw new CustomException("comment not found");
            }

            return comment;
        }

        private async Task<bool> UserLikePublicationAsync(Guid publicationId, Guid userId)
        {
            return await _context.LikesPublications
                .AnyAsync(l => l.PublicationId == publicationId 
                          && l.UserId == userId && l.DeletedDate == null);
        }

        private async Task<bool> UserLikeCommentAsync(Guid commentId, Guid userId)
        {
            return await _context.LikesComments
                .AnyAsync(l => l.CommentId == commentId
                               && l.UserId == userId && l.DeletedDate == null);
        }
    }
}
