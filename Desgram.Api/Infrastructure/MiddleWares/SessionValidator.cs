using Desgram.DAL;
using Desgram.SharedKernel.Exceptions.UnauthorizedExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Infrastructure.Middlewares
{
    public class SessionValidator
    {
        private readonly RequestDelegate _next;

        public SessionValidator(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationContext applicationContext)
        {
            var sessionIdString = context.User.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.SessionId)?.Value;
            if (!Guid.TryParse(sessionIdString, out var sessionId) ||
                await applicationContext.UserSessions
                    .FirstOrDefaultAsync(u => u.Id == sessionId) is { IsActive: true })
            {
                await _next(context);
            }
            else
            {
                throw new SessionIsNotActiveException();
            }

        }
    }
}
