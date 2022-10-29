using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class UserManager:IUserManager
    {
        private readonly ApplicationContext _context;

        public UserManager(ApplicationContext context)
        {
            _context = context;

        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(User user)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name) != null)
            {
                throw new CustomException($"username {user.Name} busy");
            }

            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email) != null)
            {
                throw new CustomException($"email {user.Email} busy");
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
