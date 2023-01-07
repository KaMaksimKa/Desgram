using AutoMapper;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Comment;
using Desgram.Api.Models.Notification;
using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel;
using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class CustomMapperService:ICustomMapperService
    {
        private readonly IMapper _mapper;

        public CustomMapperService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<PostModel>> ToPostModelsList(IQueryable<Post> posts, Guid requestorId)
        {

            var postModels = await posts.Select(p => new PostModel()
            {
                Id = p.Id,
                Description = p.Description,
                IsCommentsEnabled = p.IsCommentsEnabled,
                IsLikesVisible = p.IsLikesVisible,
                CreatedDate = p.CreatedDate,
                HasEdit = p.UpdatedDate != null,
                AmountComments = !p.IsCommentsEnabled ? null : p.Comments.Count(c => c.DeletedDate == null),
                AmountLikes = !p.IsLikesVisible ? null : p.Likes.Count(l => l.DeletedDate == null),
                HasLiked = p.Likes.Any(l => l.DeletedDate == null && l.UserId == requestorId),
                User = new PartialUserModel()
                {
                    Id = p.User.Id,
                    Name = p.User.Name,
                    Avatar = p.User.Avatars.Where(a => a.DeletedDate == null)
                        .SelectMany(a => a.ImageCandidates.Select(i => new ImageWithUrlModel()
                        {
                            Id = i.Id,
                            Height = i.Height,
                            Width = i.Width,
                            MimeType = i.MimeType,
                        })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())
                },
                ImageContents = p.ImagePostContents.Select(c => new ImageContentModel()
                {
                    ImageCandidates = c.ImageCandidates.Select(i => new ImageWithUrlModel()
                    {
                        Id = i.Id,
                        Height = i.Height,
                        Width = i.Width,
                        MimeType = i.MimeType,
                    }).ToList()
                }).ToList(),



            }).ToListAsync();

            return postModels.Select(p => _mapper.Map<PostModel>(p)).ToList();
        }

        public async Task<List<CommentModel>> ToCommentModelsList(IQueryable<Comment> comments, Guid requestorId)
        {
            var commentModels = await comments.Select(c=>new CommentModel()
            {
                Id = c.Id,
                Content = c.Content,
                CreatedDate = c.CreatedDate,
                HasEdit = c.UpdatedDate!=null,
                HasLiked = c.Likes.Any(l => l.DeletedDate == null && l.UserId == requestorId),
                AmountLikes = c.Likes.Count(l => l.DeletedDate == null),
                User = new PartialUserModel()
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    Avatar = c.User.Avatars.Where(a => a.DeletedDate == null)
                        .SelectMany(a => a.ImageCandidates.Select(i => new ImageWithUrlModel()
                        {
                            Id = i.Id,
                            Height = i.Height,
                            Width = i.Width,
                            MimeType = i.MimeType,
                        })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())
                }
            }).ToListAsync();
            return commentModels.Select(c => _mapper.Map<CommentModel>(c)).ToList();
        }

        public async Task<List<UserModel>> ToUserModelsList(IQueryable<User> users, Guid requestorId)
        {
     
            var userModels = await users.Select(u=>new UserModel()
            {
                Id = u.Id,
                Name = u.Name,
                Biography = u.Biography,
                IsPrivate = u.IsPrivate,
                FullName = u.FullName,
                AmountFollowers = u.Followers.Count(sub => sub.DeletedDate == null && sub.IsApproved),
                AmountFollowing = u.Following.Count(sub => sub.DeletedDate == null && sub.IsApproved),
                AmountPosts = u.Posts.Count(sub => sub.DeletedDate == null),
                FollowedByViewer = u.Followers
                    .Any(f => f.DeletedDate == null && f.FollowerId == requestorId && f.IsApproved),
                FollowsViewer = u.Following
                    .Any(f => f.DeletedDate == null && f.ContentMakerId == requestorId && f.IsApproved),
                HasRequestedViewer = u.Followers
                    .Any(f => f.DeletedDate == null && f.FollowerId == requestorId && !f.IsApproved),
                HasBlockedViewer = u.BlockedUsers
                    .Any(f => f.DeletedDate == null && f.BlockedId == requestorId),
                BlockedByViewer = u.UsersBlockedMe
                    .Any(f => f.DeletedDate == null && f.UserId == requestorId),
                Avatar = u.Avatars.Where(a=>a.DeletedDate==null).Select(c => new ImageContentModel()
                {
                    ImageCandidates = c.ImageCandidates.Select(i => new ImageWithUrlModel()
                    {
                        Id = i.Id,
                        Height = i.Height,
                        Width = i.Width,
                        MimeType = i.MimeType,
                    }).ToList()
                }).FirstOrDefault(),

            }).ToListAsync();

            return userModels.Select(u => _mapper.Map<UserModel>(u)).ToList();
        }

        public async Task<List<NotificationModel>> ToNotificationModelsList(IQueryable<Notification> notifications)
        {
            var notificationModels = await notifications.Select(n=>new NotificationModel()
            {
                CreatedDate = n.CreatedDate,
                LikePost = n.LikePost==null?null:new LikePostNotificationModel()
                {
                    User = new PartialUserModel()
                    {
                        Id = n.LikePost.User.Id,
                        Name = n.LikePost.User.Name,
                        Avatar = n.LikePost.User.Avatars.Where(a => a.DeletedDate == null)
                            .SelectMany(a => a.ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())
                    },
                    Post = new PartialPostModel()
                    {
                        Id = n.LikePost.PostId,
                        PreviewImage = n.LikePost.Post.ImagePostContents.First(a => a.DeletedDate == null)
                            .ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            }).First(i => i.Width == ImageWidths.Widths.Min())
                    }
                },
                LikeComment = n.LikeComment == null ? null : new LikeCommentNotificationModel()
                {
                    Comment = n.LikeComment.Comment.Content,
                    User = new PartialUserModel()
                    {
                        Id = n.LikeComment.User.Id,
                        Name = n.LikeComment.User.Name,
                        Avatar = n.LikeComment.User.Avatars.Where(a => a.DeletedDate == null)
                            .SelectMany(a=>a.ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())

                    },
                    Post = new PartialPostModel()
                    {
                        Id = n.LikeComment.Comment.PostId,
                        PreviewImage = n.LikeComment.Comment.Post.ImagePostContents.First(a => a.DeletedDate == null)
                            .ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            }).First(i => i.Width == ImageWidths.Widths.Min())
                    }
                },
                Comment = n.Comment == null ? null : new CommentNotificationModel()
                {
                    Content = n.Comment.Content,
                    User = new PartialUserModel()
                    {
                        Id = n.Comment.User.Id,
                        Name = n.Comment.User.Name,
                        Avatar = n.Comment.User.Avatars.Where(a => a.DeletedDate == null)
                            .SelectMany(a => a.ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())
                    },
                    Post = new PartialPostModel()
                    {
                        Id = n.Comment.PostId,
                        PreviewImage = n.Comment.Post.ImagePostContents.First(a => a.DeletedDate == null)
                            .ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            }).First(i => i.Width == ImageWidths.Widths.Min())
                    }
                },
                Subscription = n.Subscription == null ? null : new SubscriptionNotificationModel()
                {
                    User = new PartialUserModel()
                    {
                        Id = n.Subscription.Follower.Id,
                        Name = n.Subscription.Follower.Name,
                        Avatar = n.Subscription.Follower.Avatars.Where(a => a.DeletedDate == null)
                            .SelectMany(a => a.ImageCandidates.Select(i => new ImageWithUrlModel()
                            {
                                Id = i.Id,
                                Height = i.Height,
                                Width = i.Width,
                                MimeType = i.MimeType,
                            })).FirstOrDefault(i => i.Width == ImageWidths.Widths.Min())
                    },
                    IsApproved = n.Subscription.IsApproved
                },
                HasViewed = n.HasViewed,
                
            }).ToListAsync();

            return notificationModels.Select(n => _mapper.Map<NotificationModel>(n)).ToList();
        }
    }
}
