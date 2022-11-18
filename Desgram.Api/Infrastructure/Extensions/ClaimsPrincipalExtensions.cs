using Desgram.SharedKernel.Exceptions;
using System.Security.Claims;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Desgram.SharedKernel.Exceptions.UnauthorizedExceptions;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userGuid))
            {
                throw new UnauthorizedException();
            }

            return userGuid;
        }

        public static Guid GetSessionId(this ClaimsPrincipal principal)
        {
            var sessionIdString = principal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.SessionId)?.Value;
            if (!Guid.TryParse(sessionIdString, out var sessionGuid))
            {
                throw new SessionNotFoundException();
            }

            return sessionGuid;
        }

        public static Guid GetRefreshTokenId(this ClaimsPrincipal principal)
        {
            var guidString = principal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.RefreshTokenId)?.Value;

            if (!Guid.TryParse(guidString, out var guid))
            {
                throw new UnauthorizedException();
            }

            return guid;
        }
    }
}
