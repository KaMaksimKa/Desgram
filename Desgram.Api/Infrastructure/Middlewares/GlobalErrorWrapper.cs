using Desgram.DAL;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Desgram.SharedKernel.Exceptions.UnauthorizedExceptions;
using static System.Net.Mime.MediaTypeNames;

namespace Desgram.Api.Infrastructure.Middlewares
{
    public class GlobalErrorWrapper
    {
        private readonly RequestDelegate _next;

        public GlobalErrorWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityAlreadyExistsException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                    e.EntityName,
                });
            }
            catch (InvalidPropertyException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                    e.PropertyName,
                });
            }
            catch (UnauthorizedException e)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                });
            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                    e.EntityName
                });
            }
            catch (ForbiddenException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                });
            }
            catch (BadRequestException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    e.Message,
                });
            }
        }
    }
}
