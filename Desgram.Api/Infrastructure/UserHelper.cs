using Desgram.SharedKernel.Exceptions;
using System.Security.Claims;

namespace Desgram.Api.Infrastructure
{
    public static class UserHelper
    {
        public static Guid GetUserIdByClaimsPrincipal(ClaimsPrincipal principal)
        {
            var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userGuid))
            {
                throw new CustomException("you are not authorize");
            }

            return userGuid;
        }
    }
}
