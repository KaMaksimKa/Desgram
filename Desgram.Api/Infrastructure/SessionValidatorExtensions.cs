namespace Desgram.Api.Infrastructure
{
    public static class SessionValidatorExtensions
    {
        public static IApplicationBuilder UseUserSessionValidator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionValidator>();
        }
    }
}
