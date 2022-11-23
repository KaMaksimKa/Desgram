using Desgram.Api.Config;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Desgram.Api.Infrastructure
{
    /// <summary>
    /// Записывает начальные необходимые данные, в данном случае добавляет пользователя с правами администратора
    /// </summary>
    public static class DataInitializer
    {
        public static async Task CreateAdminUser(IOptions<AdminUserConfig> config, ApplicationContext context)
        {
            if (await context.Users.AnyAsync(u => u.Name.ToLower() == config.Value.Name.ToLower()))
            {
                return;
            }

            var user = (await context.Users.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = config.Value.Name,
                Email = config.Value.Email,
                PasswordHash = HashHelper.GetHash(config.Value.Password),
                Roles = new List<ApplicationRole>(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime
            })).Entity;

            foreach (var roleName in config.Value.Roles)
            {
                var role = await context.ApplicationRoles
                    .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());

                if (role == null)
                {
                    role = (await context.ApplicationRoles.AddAsync(new ApplicationRole()
                    {
                        Name = roleName
                    })).Entity;
                }

                user.Roles.Add(role);
            }

            await context.SaveChangesAsync();

        }
    }
}
