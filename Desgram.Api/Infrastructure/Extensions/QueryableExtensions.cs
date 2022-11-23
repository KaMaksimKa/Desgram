using AutoMapper.QueryableExtensions;
using Desgram.Api.Mapper;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ProjectToByRequestorId<T>(this IQueryable queryable, AutoMapper.IConfigurationProvider provider, Guid requestorId)
        {
            MapperProfile.RequestorId = requestorId;
            return queryable.ProjectTo<T>(provider);
        }
    }
}
