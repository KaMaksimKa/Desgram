using System.Security.Claims;

namespace Desgram.Api.Infrastructure
{
    public static class UserHelperExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            return UserHelper.GetUserIdByClaimsPrincipal(principal);
        }
    }
}
