using System.Xml.Linq;

namespace Desgram.DAL.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public  bool HasViewed { get; set; }



        public virtual User User { get; set; } = null!;

        /*public Guid LikePostId { get; set; }
        public virtual LikePost? LikePost { get; set; } = null!;*/
        /*Не смотря что тут нужна связь one to one я использую one to many, потому что иначе automapper
         выдает ошибку которая вот прям не должна появляться, а именно она появляется если глубина маппинга достигает
        какой то определленой глубины
        (Сама ошибка при использование one to one:System.ArgumentException: Property 'Desgram.DAL.Entities.LikePost LikePost' is not defined for type 'Desgram.DAL.Entities.LikePost' (Parameter 'property')
           at System.Linq.Expressions.Expression.Property(Expression expression, PropertyInfo property)
           at System.Linq.Expressions.Expression.MakeMemberAccess(Expression expression, MemberInfo member)
           at System.Linq.Expressions.MemberExpression.Update(Expression expression)
           at System.Linq.Expressions.ExpressionVisitor.VisitMember(MemberExpression node)
           at System.Linq.Expressions.MemberExpression.Accept(ExpressionVisitor visitor)
           at System.Linq.Expressions.ExpressionVisitor.VisitMember(MemberExpression node)
           at System.Linq.Expressions.MemberExpression.Accept(ExpressionVisitor visitor)
           at AutoMapper.QueryableExtensions.Impl.ProjectionBuilder.FirstPassLetPropertyMaps.SubQueryPath.GetSourceExpression(Expression parameter)
           at AutoMapper.QueryableExtensions.Impl.ProjectionBuilder.FirstPassLetPropertyMaps.<>c__DisplayClass11_0.<GetSubQueryExpression>b__0(SubQueryPath path)
           at System.Linq.Enumerable.SelectListIterator`2.ToArray()
           at AutoMapper.QueryableExtensions.Impl.ProjectionBuilder.FirstPassLetPropertyMaps.GetSubQueryExpression(ProjectionBuilder builder, Expression projection, TypeMap typeMap, ProjectionRequest& request, ParameterExpression instanceParameter)
           at AutoMapper.QueryableExtensions.Impl.ProjectionBuilder.CreateProjection(ProjectionRequest& request, LetPropertyMaps letPropertyMaps)
           at AutoMapper.Internal.LockingConcurrentDictionary`2.<>c__DisplayClass2_1.<.ctor>b__1()
           at System.Lazy`1.ViaFactory(LazyThreadSafetyMode mode)
           at System.Lazy`1.ExecutionAndPublication(LazyHelper executionAndPublication, Boolean useDefaultConstructor)
           at System.Lazy`1.CreateValue()
           at AutoMapper.QueryableExtensions.Impl.ProjectionBuilder.GetProjection(Type sourceType, Type destinationType, Object parameters, MemberPath[] membersToExpand)
           at AutoMapper.QueryableExtensions.Extensions.ToCore(IQueryable source, Type destinationType, IConfigurationProvider configuration, Object parameters, IEnumerable`1 memberPathsToExpand)
           at AutoMapper.QueryableExtensions.Extensions.ToCore[TResult](IQueryable source, IConfigurationProvider configuration, Object parameters, IEnumerable`1 memberPathsToExpand)
           at AutoMapper.QueryableExtensions.Extensions.ProjectTo[TDestination](IQueryable source, IConfigurationProvider configuration, Expression`1[] membersToExpand)
           at Desgram.Api.Services.NotificationService.GetNotificationsAsync(SkipDateTakeModel model, Guid requestorId) in C:\Users\karag\source\repos\DDSchool\Desgram\Desgram.Api\Services\NotificationService.cs:line 137
           at Desgram.Api.Controllers.NotificationController.GetNotifications(SkipDateTakeModel model) in C:\Users\karag\source\repos\DDSchool\Desgram\Desgram.Api\Controllers\NotificationController.cs:line 23
           at lambda_method239(Closure , Object )
           at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeInnerFilterAsync()
        --- End of stack trace from previous location ---
           at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
           at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
           at Microsoft.AspNetCore.Routing.EndpointMiddleware.<Invoke>g__AwaitRequestTask|6_0(Endpoint endpoint, Task requestTask, ILogger logger)
           at Desgram.Api.Infrastructure.Middlewares.SessionValidator.InvokeAsync(HttpContext context, ApplicationContext applicationContext) in C:\Users\karag\source\repos\DDSchool\Desgram\Desgram.Api\Infrastructure\Middlewares\SessionValidator.cs:line 23
           at Desgram.Api.Infrastructure.Middlewares.GlobalErrorWrapper.InvokeAsync(HttpContext context) in C:\Users\karag\source\repos\DDSchool\Desgram\Desgram.Api\Infrastructure\Middlewares\GlobalErrorWrapper.cs:line 23
           at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
           at AspNetCoreRateLimit.RateLimitMiddleware`1.Invoke(HttpContext context) in C:\Users\User\Documents\Github\AspNetCoreRateLimit\src\AspNetCoreRateLimit\Middleware\RateLimitMiddleware.cs:line 124
           at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
           at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
           at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
           at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context))*/
        public Guid? LikePostId { get; set; } = null!;
        public Guid? LikeCommentId { get; set; } = null!;
        public Guid? CommentId { get; set; } = null!;
        public Guid? SubscriptionId { get; set; } = null!;

        public virtual LikePost? LikePost { get; set; } = null!;
        public virtual LikeComment? LikeComment { get; set; } = null!;
        public virtual Comment? Comment { get; set; } = null!;
        public virtual UserSubscription? Subscription { get; set; } = null!;
    }
}
