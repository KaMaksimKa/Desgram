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
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)));
            CreateMap<User, UserModel>();
            CreateMap<Publication, PublicationModel>()
                .ForMember(d => d.ImageGuidList, m => m.MapFrom(s => s.ImagesPublication.Select(i => i.Id).ToList()));
        }
    }
}
