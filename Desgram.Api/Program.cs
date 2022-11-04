using Desgram.Api;
using Desgram.Api.Config;
using Desgram.Api.Infrastructure;
using Desgram.Api.Services;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var authSection = builder.Configuration.GetSection(AuthConfig.Position);
var authConfig = authSection.Get<AuthConfig>();

builder.Services.Configure<AuthConfig>(authSection);

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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachService,AttachService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPublicationService, PublicationService>();


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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseUserSessionValidator();

app.MapControllers();

app.Run();
