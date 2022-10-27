using AutoMapper;
using Desgram.Api.Models;
using Desgram.DAL.Entities;
using SharedKernel;

namespace Desgram.Api
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserDTO, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)));
            CreateMap<User, UserDTO>();
        }
    }
}
