using AutoMapper;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Blocked;
using Desgram.Api.Models.Comment;
using Desgram.Api.Models.Post;
using Desgram.Api.Models.User;
using Desgram.DAL.Entities;
using SharedKernel;

namespace Desgram.Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.Now.UtcDateTime));

            CreateMap<User, UserModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(
                    s => s.Avatars.FirstOrDefault(a => a.DeletedDate == null)))
                .ForMember(d => d.AmountFollowers, m => m.MapFrom(
                    s => s.Followers.Count(sub => sub.DeletedDate == null && sub.IsApproved)))
                .ForMember(d => d.AmountFollowing, m => m.MapFrom(
                    s => s.Following.Count(sub => sub.DeletedDate == null && sub.IsApproved)))
                .ForMember(d => d.AmountPosts, m => m.MapFrom(
                    s => s.Posts.Count(sub => sub.DeletedDate == null)));

            CreateMap<User, PartialUserModel>()
                .ForMember(d => d.Avatar, m => m.MapFrom(
                    s => s.Avatars.FirstOrDefault(a => a.DeletedDate == null)));


            CreateMap<AttachPost, AttachWithUrlModel>();
            CreateMap<Avatar, AttachWithUrlModel>();
            CreateMap<Attach, AttachWithPathModel>();

            CreateMap<Post, PostModel>()
                .ForMember(d => d.User, m => m.MapFrom(s => s.User))
                .ForMember(d => d.AttachesPost, m => m.MapFrom(
                        s => s.Attaches))
                .ForMember(d => d.HashTags, m => m.MapFrom(
                        s => s.HashTags.Select(h => h.Title).ToList()))
                .ForMember(d => d.AmountComments, m => m.MapFrom<int?>(
                    s => !s.IsCommentsEnabled ? null : s.Comments.Count(c => c.DeletedDate == null)))
                .ForMember(d => d.AmountLikes, m => m.MapFrom<int?>(
                    s => !s.IsLikesVisible ? null : s.Likes.Count(l => l.DeletedDate == null)));


            CreateMap<Comment, CommentModel>()
                .ForMember(d=>d.User,m=>m.MapFrom(s=>s.User))
                .ForMember(d => d.AmountLikes, m => m.MapFrom(s => s.Likes.Count(l => l.DeletedDate == null)))
                .ForMember(d => d.IsEdit, m => m.MapFrom(s => s.UpdatedDate != null));


            CreateMap<BlockingUser, BlockedUserModel>();


        }

    }
}
