using AutoMapper;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Post;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;


namespace Desgram.Api.Services
{
    public class PostService:IPostService
    {
        private readonly ApplicationContext _context;
        private readonly IAttachService _attachService;
        private readonly IMapper _mapper;

        public PostService(ApplicationContext context,IAttachService attachService,
            IMapper mapper)
        {
            _context = context;
            _attachService = attachService;
            _mapper = mapper;
        }
        
        public async Task CreatePostAsync(CreatePostModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            
            var hashTags = await GetHashTags(model.Description);

            /* Пока что в роли пост контента могут выступать файлы которые конвертируются к jpeg(такие как jpeg,png и тд.)
             В будующем планируется добавить видео*/
            var imagePostContents = new List<PostImageContent>();
            foreach (var metadata in model.MetadataModels)
            {
                imagePostContents.Add(new PostImageContent()
                {
                    ImageCandidates = (await _attachService.FromTempToImage(metadata))
                        .Select(c=>new Image()
                        {
                            CreatedDate = DateTimeOffset.Now.UtcDateTime,
                            DeletedDate = null,
                            Name = metadata.Name,
                            MimeType = c.MimeType,
                            Id = c.Id,
                            Path = c.Path,
                            Owner = user,
                            Height = c.Height,
                            Width = c.Width,
                            
                        })
                        .ToList(),
                    CreatedDate = DateTimeOffset.Now.UtcDateTime,
                    DeletedDate = null,
                });
            }

            var post = new Post()
            {
                Id = Guid.NewGuid(),
                User = user,
                Description = model.Description,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                IsCommentsEnabled = model.IsCommentsEnabled,
                IsLikesVisible = model.IsLikesVisible,
                Comments = new List<Comment>(),
                Likes = new List<LikePost>(),
                ImagePostContents = imagePostContents,
                HashTags = hashTags,
                DeletedDate = null
            };

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid postId, Guid requestorId)
        {
            var post = await _context.Posts.GetPostByIdAsync(postId);

            if (post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }
            
            post.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(UpdatePostModel model, Guid requestorId)
        {
            var post = await GetPostWithTagsByIdAsync(model.PostId);

            if (post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            var hashtags = await GetHashTags(model.Description);

            post.Description = model.Description;
            post.UpdatedDate = DateTimeOffset.Now.UtcDateTime;

            foreach (var hashtag in post.HashTags.ToList().Except(hashtags))
            {
                post.HashTags.Remove(hashtag);
            }
            foreach (var hashtag in hashtags.Except(post.HashTags))
            {
                post.HashTags.Add(hashtag);
            }


            await _context.SaveChangesAsync();
        }

        public async Task ChangeLikesVisibilityAsync(ChangeLikesVisibilityModel model, Guid requestorId)
        {
            var post = await _context.Posts.GetPostByIdAsync(model.PostId);

            if (post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            post.IsLikesVisible = model.IsLikesVisible;

            await _context.SaveChangesAsync();

        }

        public async Task ChangeIsCommentsEnabledAsync(ChangeIsCommentsEnabledModel model, Guid requestorId)
        {
            var post = await _context.Posts.GetPostByIdAsync(model.PostId);

            if (post.UserId != requestorId)
            {
                throw new AuthorContentException();
            }

            post.IsCommentsEnabled = model.IsCommentsEnabled;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PostModel>> GetAllPostsAsync(PostRequestModel model, Guid requestorId)
        {
            var posts = await _context.Posts.AsNoTracking()
                .Where(p => p.DeletedDate == null && !p.User.IsPrivate
                            && !p.User.BlockedUsers.Any(u => u.BlockedId == requestorId && u.DeletedDate == null))
                .Skip(model.Skip).Take(model.Take)
                .ProjectToByRequestorId<PostModel>(_mapper.ConfigurationProvider, requestorId)
                .ToListAsync();

            return posts.Select(p => _mapper.Map<PostModel>(p)).ToList();
        }

        public async Task<List<PostModel>> GetPostByHashTagAsync(PostByHashtagRequestModel model,Guid requestorId)
        {
            var posts = await _context.Posts.AsNoTracking()
                .Where(p => p.DeletedDate == null && !p.User.IsPrivate 
                            && !p.User.BlockedUsers.Any(u=>u.BlockedId == requestorId && u.DeletedDate == null))
                .Where(p => p.HashTags.Any(tag => tag.Title == model.Hashtag) )
                .Skip(model.Skip).Take(model.Take)
                .ProjectToByRequestorId<PostModel>(_mapper.ConfigurationProvider, requestorId)
                .ToListAsync();

            return posts.Select(p => _mapper.Map<PostModel>(p)).ToList();
        }

        public async Task<List<PostModel>> GetSubscriptionsFeedAsync(PostRequestModel model,Guid requestorId)
        {

            var posts = await _context.UserSubscriptions.AsNoTracking()
                .Where(s=>s.FollowerId == requestorId && s.DeletedDate == null && s.IsApproved)
                .Select(s=>s.ContentMaker)
                .SelectMany(u=>u.Posts)
                .Where(p=>p.DeletedDate == null)
                .Skip(model.Skip).Take(model.Take)
                .ProjectToByRequestorId<PostModel>(_mapper.ConfigurationProvider, requestorId)
                .ToListAsync();

            return posts.Select(p => _mapper.Map<PostModel>(p)).ToList();
        }

        public async Task<List<PostModel>> GetUserPostsAsync(PostByUserIdRequestModel model, Guid requestorId)
        {
            var contentMaker = await _context.Users.GetUserByIdAsync(model.UserId);

            if (contentMaker.IsPrivate &&
               !(await _context.UserSubscriptions.AnyAsync(s =>s.DeletedDate == null && s.IsApproved
               && (s.ContentMakerId == contentMaker.Id && s.FollowerId == requestorId)) || contentMaker.Id == requestorId))
            {
                throw new AccessActionException();
            }

            var posts = await _context.Users.AsNoTracking()
                .Where(u=>u.Id == model.UserId)
                .SelectMany(u => u.Posts)
                .Where(p=>p.DeletedDate == null)
                .Skip(model.Skip).Take(model.Take)
                .ProjectToByRequestorId<PostModel>(_mapper.ConfigurationProvider,requestorId)
                .ToListAsync();

            return posts.Select(p=>_mapper.Map<PostModel>(p)).ToList();
        }

        private async Task<List<HashTag>> GetHashTags(string descriptionPost)
        {
            var hashTagsString = descriptionPost.Replace("#", " #").Split()
                .Where(w => w.StartsWith("#"))
                .ToList();

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
                    var hashtag = (await _context.HashTags.AddAsync(new HashTag()
                    {
                        Id = Guid.NewGuid(),
                        Title = hashTagString
                    })).Entity;

                    hashTags.Add(hashtag);
                }

            }
            return hashTags;
        }

        private async Task<Post> GetPostWithTagsByIdAsync(Guid postId)
        {
            var post = await _context.Posts
                .Include(p => p.HashTags)
                .Where(p => p.DeletedDate == null && p.Id == postId)
                .FirstOrDefaultAsync();
            if (post == null)
            {
                throw new PostNotFoundException();
            }

            return post;
        }
    }
}
