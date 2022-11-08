namespace Desgram.Api.Infrastructure.MiddleWares
{
    public static class SessionValidatorExtensions
    {
        public static IApplicationBuilder UseUserSessionValidator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionValidator>();
        }
    }
}
