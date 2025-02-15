using AspNetCoreRateLimit;
using Desgram.Api;
using Desgram.Api.Config;
using Desgram.Api.Infrastructure;
using Desgram.Api.Infrastructure.Middlewares;
using Desgram.Api.Mapper;
using Desgram.Api.Services;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var authSection = builder.Configuration.GetSection(AuthConfig.Position);
var authConfig = authSection.Get<AuthConfig>();
builder.Services.Configure<AuthConfig>(authSection);

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(EmailConfig.Position));
builder.Services.Configure<AdminUserConfig>(builder.Configuration.GetSection(AdminUserConfig.Position));
builder.Services.Configure<PushConfig>(builder.Configuration.GetSection(PushConfig.Position));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,new OpenApiSecurityScheme()
    {
        Description = "������� Access token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth" });
    options.SwaggerDoc("Api", new OpenApiInfo { Title = "Api" });
    options.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin" });
} );

var connectionString = builder.Configuration.GetConnectionString(Constants.ConnectionStringNames.PostgresSql);
builder.Services.AddDbContext<ApplicationContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(connectionString,contextOptionsBuilder =>
    {
        contextOptionsBuilder.MigrationsAssembly("Desgram.Api");
    });
});

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();



builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachService,AttachService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUrlService,UrlService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IImageEditor, ImageEditor>();
builder.Services.AddScoped<IBlockingService, BlockingService>();
builder.Services.AddScoped<IConfirmService, ConfirmService>();
builder.Services.AddScoped<AttachModelMapperAction>();
builder.Services.AddScoped<IPushService, PushService>();
builder.Services.AddScoped<IGooglePushService, GooglePushService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICustomMapperService,CustomMapperService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        { 
            ValidateIssuer = true,
            ValidIssuer = authConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = authConfig.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = authConfig.SymmetricSecurityKey,
            ClockSkew = TimeSpan.Zero
        };
    } );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidAccessToken", policyBuilder =>
    {
        policyBuilder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policyBuilder.RequireAuthenticatedUser();
    } );
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationContext>();
    if (context != null)
    {
        await context.Database.MigrateAsync();
    
    }
}


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationContext>();
    var config = scope.ServiceProvider.GetService<IOptions<AdminUserConfig>>();
    if (config == null)
    {
        throw new NullReferenceException(nameof(IOptions<AdminUserConfig>));
    }
    if (context == null)
    {
        throw new NullReferenceException(nameof(ApplicationContext));
    }

    await DataInitializer.CreateAdminUser(config,context);
}


/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("Api/swagger.json", "Api");
        options.SwaggerEndpoint("Auth/swagger.json", "Auth");
        options.SwaggerEndpoint("Admin/swagger.json", "Admin");
    } );
/*}*/

/*app.UseHttpsRedirection();*/

app.UseAuthentication();

app.UseClientRateLimiting();

app.UseAuthorization();

app.UseGlobalErrorWrapper();
app.UseUserSessionValidator();

app.MapControllers();

app.Run();
