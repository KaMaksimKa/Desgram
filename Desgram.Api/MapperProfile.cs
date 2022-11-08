using AutoMapper;
using Desgram.Api.Models;
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
            
            CreateMap<User, UserModel>();

            CreateMap<Publication, PublicationModel>()
                .ForMember(d => d.ContentModels,
                    m => m.MapFrom(
                        s => s.AttachPublications.Select(i => new PublicationContentModel()
                        {
                            Id = i.Id,
                            Url =$"api/Attach/DisplayAttachById?id={i.Id}",
                            MimeType = i.MimeType
                        }).ToList()))
                .ForMember(d=>d.UserName,
                    m=>m.MapFrom(
                        s =>s.User.Name))
                .ForMember(d => d.HashTags,
                    m => m.MapFrom(
                        s => s.HashTags.Select(h => h.Title).ToList()));


           

            CreateMap<Comment, CommentModel>();
        }
    }
}
