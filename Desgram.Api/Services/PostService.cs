using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Post;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Desgram.Api.Services
{
    public class PostService:IPostService
    {
        private readonly ApplicationContext _context;
        private readonly IAttachService _attachService;
        private readonly IMapper _mapper;
        private readonly IUrlService _urlService;

        public PostService(ApplicationContext context,IAttachService attachService,
            IMapper mapper,IUrlService urlService)
        {
            _context = context;
            _attachService = attachService;
            _mapper = mapper;
            _urlService = urlService;
        }
        
        public async Task CreatePostAsync(CreatePostModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);
            var hashTagsString = model.Description.Split().Where(w => w.StartsWith("#")).ToList();
            var hashTags = await GetHashTags(hashTagsString);

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
                Attaches = model.MetadataModels
                    .Select(meta => new AttachPost()
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

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(Guid postId, Guid userId)
        {
            var post = await _context.Posts.GetPostById(postId);

            if (post.UserId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }
            
            post.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(UpdatePostModel model, Guid userId)
        {
            var post = await _context.Posts.GetPostById(model.PostId);

            if (post.UserId != userId)
            {
                throw new CustomException("you don't have enough right");
            }

            post.Description = model.Description;
            post.UpdatedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeLikesVisibilityAsync(ChangeLikesVisibilityModel model, Guid userId)
        {
            var post = await _context.Posts.GetPostById(model.PostId);

            if (post.UserId != userId)
            {
                throw new CustomException("you don't have enough right");
            }

            post.IsLikesVisible = model.IsLikesVisible;

            await _context.SaveChangesAsync();

        }

        public async Task ChangeIsCommentsEnabledAsync(ChangeIsCommentsEnabledModel model, Guid userId)
        {
            var post = await _context.Posts.GetPostById(model.PostId);

            if (post.UserId != userId)
            {
                throw new CustomException("you don't have enough right");
            }

            post.IsCommentsEnabled = model.IsCommentsEnabled;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PostModel>> GetAllPostsAsync(PostRequestModel model, Guid userId)
        {
            var posts =(await _context.Posts
                .GetAvailablePostsByUserId(userId)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PostModel>(_mapper.ConfigurationProvider)
                .ToListAsync());

            AfterMapPost(posts,userId);

            return posts;
        }

        public async Task<List<PostModel>> GetPostByHashTagAsync(PostByHashtagRequestModel model,Guid userId)
        {
            var posts = await _context.Posts
                .GetAvailablePostsByUserId(userId)
                .Where(p => p.HashTags.Any(tag => tag.Title == model.Hashtag))
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PostModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            AfterMapPost(posts,userId);

            return posts;
        }

        public async Task<List<PostModel>> GetSubscriptionsFeedAsync(PostRequestModel model,Guid userId)
        {

            var posts = await _context.UserSubscriptions
                .Where(s=>s.FollowerId == userId && s.DeletedDate == null && s.IsApproved)
                .Select(s=>s.ContentMaker)
                .SelectMany(u=>u.Posts)
                .Where(p=>p.DeletedDate == null)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PostModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            AfterMapPost(posts, userId);

            return posts;
        }

        public async Task<List<PostModel>> GetPostsByUserIdAsync(PostByUserIdRequestModel model, Guid userId)
        {
            var contentMaker = await _context.Users.GetUserByIdAsync(model.UserId);

            if (contentMaker.IsPrivate &&
               !(await _context.UserSubscriptions.AnyAsync(s =>s.DeletedDate == null && s.IsApproved
               && (s.ContentMakerId == contentMaker.Id && s.FollowerId == userId)) || contentMaker.Id == userId))
            {
                throw new CustomException("you don't have enough rights");
            }

            var publications =await _context.Users
                .Where(u=>u.Id == model.UserId)
                .SelectMany(u => u.Posts)
                .Where(p=>p.DeletedDate == null)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PostModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            AfterMapPost(publications,userId);

            return publications;
        }

        private void AfterMapPost(List<PostModel> posts, Guid userId)
        {
            SetUrlForAttachPost(posts);
            SetIsLiked(posts, userId);
        }

        private void SetIsLiked(List<PostModel> posts,Guid userId)
        {
            var postLikes = _context.Posts
                .Where(p=>posts.Select(pub=>pub.Id).Contains(p.Id) 
                          && p.Likes.Any(l=>l.UserId == userId && l.DeletedDate == null))
                .Select(p => p.Id);

            foreach (var postModel in posts.Where(p=>postLikes.Contains(p.Id)))
            {
                postModel.IsLiked = true;
            }
        }

        private void SetUrlForAttachPost(List<PostModel> posts)
        {
            foreach (var post in posts)
            {
                foreach (var contentModel in post.AttachesPost)
                {
                    contentModel.Url = _urlService.GetUrlDisplayAttachById(contentModel.Id);
                }
            }
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

    }
}
