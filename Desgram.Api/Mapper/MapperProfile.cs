using AutoMapper;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Comment;
using Desgram.Api.Models.Notification;
using Desgram.Api.Models.Post;
using Desgram.Api.Models.Role;
using Desgram.Api.Models.User;
using Desgram.Api.Services.ServiceModel;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;

namespace Desgram.Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TryCreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.Now.UtcDateTime))
                .ForMember(d=>d.Name,m=>m.MapFrom(s=>s.UserName));
            

            CreateMap<UserModel, UserModel>();





            CreateMap<User, PartialUserModel>()
                .ForMember(d=>d.Avatar,m=>m.MapFrom<Image?>(s=>s.Avatars.First(a => a.DeletedDate == null)
                    .ImageCandidates.First(i=>i.Width == ImageWidths.Widths.Min())));
            CreateMap<PartialUserModel, PartialUserModel>();


            CreateMap<Attach, AttachWithPathModel>();
            CreateMap<AttachWithUrlModel, AttachWithUrlModel>()
                .AfterMap<AttachModelMapperAction>();

            CreateMap<Image, ImageWithUrlModel>();
            CreateMap<ImageWithUrlModel, ImageWithUrlModel>()
                .AfterMap<AttachModelMapperAction>();

            CreateMap<PostImageContent, ImageContentModel>();
            CreateMap<Avatar, ImageContentModel>();
            CreateMap<ImageContentModel, ImageContentModel>();

            CreateMap<PostModel, PostModel>();

            CreateMap<Post, PartialPostModel>()
                .ForMember(d => d.PreviewImage, m => m.MapFrom<Image?>(s => s.ImagePostContents.First(a => a.DeletedDate == null)
                    .ImageCandidates.First(i => i.Width == ImageWidths.Widths.Min())));
            CreateMap<PartialPostModel, PartialPostModel>();

            CreateMap<CommentModel, CommentModel>();


            CreateMap<ApplicationRole, ApplicationRoleModel>();

            CreateMap<HashTag,HashtagModel>()
                .ForMember(d =>d.Hashtag,m=>m.MapFrom(s=>s.Title))
                .ForMember(d=>d.AmountPosts,m=>m.MapFrom(s=>s.Posts.Count(p => p.DeletedDate==null)));

            CreateMap<LikePost?, LikePostNotificationModel?>();
            CreateMap<LikePostNotificationModel?, LikePostNotificationModel?>();

            CreateMap<LikeComment, LikeCommentNotificationModel>()
                .ForMember(d=>d.Post,m=>m.MapFrom(s=>s.Comment.Post))
                .ForMember(d=>d.Comment,m=>m.MapFrom(s=>s.Comment.Content));
            CreateMap<LikeCommentNotificationModel, LikeCommentNotificationModel>();

            CreateMap<Comment, CommentNotificationModel>();
            CreateMap<CommentNotificationModel, CommentNotificationModel>();

            CreateMap<UserSubscription, SubscriptionNotificationModel>()
                .ForMember(d=>d.User,m=>m.MapFrom(s=>s.Follower));
            CreateMap<SubscriptionNotificationModel, SubscriptionNotificationModel>();

            /*CreateMap<Notification, NotificationModel>()
                .ForMember(d=>d.LikePost,m=>m.MapFrom(s=>s.LikesPost.FirstOrDefault()))
                .ForMember(d => d.LikeComment, m => m.MapFrom(s => s.LikesComment.FirstOrDefault()))
                .ForMember(d => d.Comment, m => m.MapFrom(s => s.Comments.FirstOrDefault()))
                .ForMember(d => d.Subscription, m => m.MapFrom(s => s.Subscriptions.FirstOrDefault()));
            
            */
            CreateMap<NotificationModel, NotificationModel>();


        }

    }
}
