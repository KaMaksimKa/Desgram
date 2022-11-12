using Desgram.SharedKernel.Exceptions;
using System.Security.Claims;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userGuid))
            {
                throw new CustomException("you are not authorize");
            }

            return userGuid;
        }

        public static Guid GetSessionId(this ClaimsPrincipal principal)
        {
            var sessionIdString = principal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.SessionId)?.Value;
            if (!Guid.TryParse(sessionIdString, out var sessionGuid))
            {
                throw new CustomException("session not found");
            }

            return sessionGuid;
        }

        public static Guid GetRefreshTokenId(this ClaimsPrincipal principal)
        {
            var guidString = principal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.RefreshTokenId)?.Value;

            if (!Guid.TryParse(guidString, out var guid))
            {
                throw new CustomException("token is invalid");
            }

            return guid;
        }
    }
}
