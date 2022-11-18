namespace Desgram.Api.Infrastructure.Middlewares
{
    public static class GlobalErrorWrapperExtensions
    {
        public static IApplicationBuilder UseGlobalErrorWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorWrapper>();
        }
    }
}
