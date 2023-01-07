using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.Post;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
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
        private readonly ICustomMapperService _customMapperService;

        public PostService(ApplicationContext context,IAttachService attachService,
            IMapper mapper,ICustomMapperService customMapperService)
        {
            _context = context;
            _attachService = attachService;
            _mapper = mapper;
            _customMapperService = customMapperService;
        }
        
        public async Task CreatePostAsync(CreatePostModel model, Guid requestorId)
        {
            if (model.MetadataModels.Count == 0)
            {
                throw new BadRequestException()
                {
                    Errors =
                    {
                        [nameof(model.MetadataModels)] = new List<string>(){ "metadataModels не может быть пустым списком" }
                    }
                };
            }
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

        public async Task EditPostAsync(UpdatePostModel model, Guid requestorId)
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

        public async Task<PostModel> GetPostByIdAsync(Guid postId, Guid requestorId)
        {
            var posts = _context.Posts.AsNoTracking()
                .Where(p => p.DeletedDate == null && p.Id == postId && (!p.User.IsPrivate || p.User.Followers.Any(u =>
                                u.FollowerId == requestorId && u.DeletedDate == null))
                            && !p.User.BlockedUsers.Any(u =>
                                u.BlockedId == requestorId && u.DeletedDate == null));
            var post = (await _customMapperService.ToPostModelsList(posts, requestorId)).FirstOrDefault();

            if (post == null)
            {
                throw new PostNotFoundException();
            }

            return post;
        }

        public async Task<List<PostModel>> GetAllPostsAsync(SkipTakeModel model, Guid requestorId)
        {
            var posts = _context.Posts.AsNoTracking()
                .Where(p => p.DeletedDate == null && !p.User.IsPrivate
                                                  && !p.User.BlockedUsers.Any(u =>
                                                      u.BlockedId == requestorId && u.DeletedDate == null))
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take);

            return await _customMapperService.ToPostModelsList(posts, requestorId);
        }


        public async Task<List<PostModel>> GetPostByHashTagAsync(PostByHashtagRequestModel model,Guid requestorId)
        {
            var posts =  _context.Posts.AsNoTracking()
                .Where(p => p.DeletedDate == null && !p.User.IsPrivate
                                                  && !p.User.BlockedUsers.Any(u =>
                                                      u.BlockedId == requestorId && u.DeletedDate == null))
                .Where(p => p.HashTags.Any(tag => tag.Title == model.Hashtag))
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take);

            return await _customMapperService.ToPostModelsList(posts, requestorId);
        }

        public async Task<List<PostModel>> GetSubscriptionsFeedAsync(SkipTakeModel model,Guid requestorId)
        {

            var posts = _context.UserSubscriptions.AsNoTracking()
                .Where(s => s.FollowerId == requestorId && s.DeletedDate == null && s.IsApproved)
                .Select(s => s.ContentMaker)
                .SelectMany(u => u.Posts)
                .Where(p => p.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take);

            return await _customMapperService.ToPostModelsList(posts, requestorId);
        }

        public async Task<List<PostModel>> GetUserPostsAsync(PostByUserIdRequestModel model, Guid requestorId)
        {
            var contentMaker = await _context.Users.GetUserByIdAsync(model.UserId);

            if ((contentMaker.IsPrivate &&
               !(await _context.UserSubscriptions.AnyAsync(s =>s.DeletedDate == null && s.IsApproved
               && (s.ContentMakerId == contentMaker.Id && s.FollowerId == requestorId)) || contentMaker.Id == requestorId)) ||
                await _context.BlockingUsers.AnyAsync(b=>b.DeletedDate == null && b.UserId ==model.UserId && b.BlockedId == requestorId))
            {
                throw new AccessActionException();
            }


            var posts =  _context.Users.AsNoTracking()
                .Where(u => u.Id == model.UserId)
                .SelectMany(u => u.Posts)
                .Where(p => p.DeletedDate == null)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(model.Skip).Take(model.Take);

            return await _customMapperService.ToPostModelsList(posts, requestorId);

        }

        public async Task<List<HashtagModel>> SearchHashtagsAsync(SearchHashtagsModel model, Guid requestorId)
        {
            return await _context.HashTags.Where(h => h.Title.Contains(model.SearchString))
                .ProjectTo<HashtagModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(h => h.AmountPosts)
                .Skip(model.Skip).Take(model.Take)
                .ToListAsync();
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
