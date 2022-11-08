using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.Publication;
using Desgram.Api.Models.Subscription;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL.Entities;
using SharedKernel;

namespace Desgram.Api
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d =>d.CreatedDate,m=> m.MapFrom(s=>DateTimeOffset.Now.UtcDateTime));

            CreateMap<User, UserModel>()
                .ForMember(s=>s.Avatar,m=>
                    m.MapFrom(d=>d.Avatar))
                .ForMember(d => d.AmountSubscribers,
                    m => m.MapFrom(
                        s => s.Subscribers.Count(sub => sub.DeletedDate == null)))
                .ForMember(d => d.AmountSubscriptions,
                    m => m.MapFrom(
                        s => s.Subscriptions.Count(sub => sub.DeletedDate == null))); ;

            CreateMap<AttachPublication, AttachWithUrlModel>();
            CreateMap<Avatar, AttachWithUrlModel>();

            CreateMap<Publication, PublicationModel>()
                .ForMember(d => d.AttachesPublication,
                    m => m.MapFrom(
                        s => s.AttachesPublication))
                .ForMember(d => d.HashTags,
                    m => m.MapFrom(
                        s => s.HashTags.Select(h => h.Title).ToList()))
                .ForMember(d => d.AmountComments,
                    m => m.MapFrom(
                        s => s.Comments.Count(c => c.DeletedDate == null)))
                .ForMember(d => d.AmountLikes,
                    m => m.MapFrom(
                        s => s.LikesPublication.Count(l => l.DeletedDate == null)));


           

            CreateMap<Comment, CommentModel>()
                .ForMember(d => d.AmountLikes,
                    m => m.MapFrom(
                        s => s.LikesComment.Count( l=> l.DeletedDate == null)));

            CreateMap<UserSubscription, SubscriptionModel>();
            
            CreateMap<UserSubscription, SubscriberModel>();

        }

    }
}
