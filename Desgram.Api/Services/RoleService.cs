using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Models.Role;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class RoleService:IRoleService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public RoleService(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateRoleAsync(string name)
        {
            if (!(await CheckRoleByNameAsync(name)))
            {
                throw new RoleAlreadyExistsException();
            }

            await _context.ApplicationRoles.AddAsync(new ApplicationRole()
            {
                Name = name,
                Users = new List<User>()
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string name)
        {

            var role = await GetRoleByNameAsync(name);

            _context.ApplicationRoles.Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task AddUserToRoleAsync(Guid userId, string roleName)
        {
            if (await IsUserInRoleAsync(userId, roleName))
            {
                return;
            }

            var role = await GetRoleByNameAsync(roleName);
            var user = await GetUserWithRolesByIdAsync(userId);

            user.Roles.Add(role);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserFromRoleAsync(Guid userId, string roleName)
        {
            if (!(await IsUserInRoleAsync(userId, roleName)))
            {
                return;
            }

            var role = await GetRoleByNameAsync(roleName);
            var user = await GetUserWithRolesByIdAsync(userId);

            user.Roles.Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserInRoleAsync(Guid userId, string roleName)
        {
            return (await GetUserWithRolesByIdAsync(userId)).Roles
                .Any(r => r.Name.ToLower() == roleName.ToLower());
        }

        public async Task<List<ApplicationRoleModel>> GetUserRoles(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .ProjectTo<ApplicationRoleModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        private async Task<User> GetUserWithRolesByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        private async Task<ApplicationRole> GetRoleByNameAsync(string roleName)
        {
            var role = await _context.ApplicationRoles
                .FirstOrDefaultAsync(r=>r.Name.ToLower() == roleName.ToLower());

            if (role == null)
            {
                throw new RoleNotFoundException();
            }

            return role;
        }

        private async Task<bool> CheckRoleByNameAsync(string roleName)
        {
            return await _context.ApplicationRoles.AnyAsync(r => r.Name.ToLower() == roleName.ToLower());
        }
    }
}
