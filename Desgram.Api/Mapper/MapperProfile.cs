using AutoMapper;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Comment;
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
        public static Guid RequestorId { get; set; }
        public MapperProfile()
        {
            CreateMap<TryCreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.Now.UtcDateTime))
                .ForMember(d=>d.Name,m=>m.MapFrom(s=>s.UserName));
            

            CreateMap<User, UserModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(
                    s => s.Avatars.FirstOrDefault(a => a.DeletedDate == null)))
                .ForMember(d => d.AmountFollowers, m => m.MapFrom(
                    s => s.Followers.Count(sub => sub.DeletedDate == null && sub.IsApproved)))
                .ForMember(d => d.AmountFollowing, m => m.MapFrom(
                    s => s.Following.Count(sub => sub.DeletedDate == null && sub.IsApproved)))
                .ForMember(d => d.AmountPosts, m => m.MapFrom(
                    s => s.Posts.Count(sub => sub.DeletedDate == null)))
                .ForMember(d=>d.FollowedByViewer,m=>m.MapFrom(d=> d.Followers
                    .Any(f => f.DeletedDate == null && f.FollowerId == RequestorId && f.IsApproved)))
                .ForMember(d => d.FollowsViewer, m => m.MapFrom(d => d.Following
                    .Any(f => f.DeletedDate == null && f.ContentMakerId == RequestorId && f.IsApproved)))
                .ForMember(d => d.HasRequestedViewer, m => m.MapFrom(d => d.Followers
                    .Any(f => f.DeletedDate == null && f.FollowerId == RequestorId && !f.IsApproved)))
                .ForMember(d => d.HasBlockedViewer, m => m.MapFrom(d => d.BlockedUsers
                    .Any(f => f.DeletedDate == null && f.BlockedId == RequestorId)))
                ;
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

            CreateMap<Post, PostModel>()
                .ForMember(d => d.User, m => m.MapFrom(s => s.User))
                .ForMember(d => d.ImageContents, m => m.MapFrom(
                        s => s.ImagePostContents))
                .ForMember(d => d.AmountComments, m => m.MapFrom<int?>(
                    s => !s.IsCommentsEnabled ? null : s.Comments.Count(c => c.DeletedDate == null)))
                .ForMember(d => d.AmountLikes, m => m.MapFrom<int?>(
                    s => !s.IsLikesVisible ? null : s.Likes.Count(l => l.DeletedDate == null)))
                .ForMember(d => d.HasLiked, m => m.MapFrom(s => s.Likes.Any(l => l.DeletedDate == null && l.UserId == RequestorId)));
                ;
            CreateMap<PostModel, PostModel>();

            CreateMap<Comment, CommentModel>()
                .ForMember(d=>d.User,m=>m.MapFrom(s=>s.User))
                .ForMember(d => d.AmountLikes, m => m.MapFrom(s => s.Likes.Count(l => l.DeletedDate == null)))
                .ForMember(d => d.HasEdit, m => m.MapFrom(s => s.UpdatedDate != null))
                .ForMember(d=>d.HasLiked,m=>m.MapFrom(s=>s.Likes.Any(l=>l.DeletedDate==null && l.UserId == RequestorId)));
            CreateMap<CommentModel, CommentModel>();


            CreateMap<ApplicationRole, ApplicationRoleModel>();



        }

    }
}
