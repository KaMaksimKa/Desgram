using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryableUserSessionExtensions
    {
        public static async Task<UserSession> GetSessionById(this IQueryable<UserSession> userSessions,Guid sessionId)
        {
            var session = await userSessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.IsActive);
            if (session == null)
            {
                throw new SessionNotFoundException();
            }

            return session;
        }
    }
}
