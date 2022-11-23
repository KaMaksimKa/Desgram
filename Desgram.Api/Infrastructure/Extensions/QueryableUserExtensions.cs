using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Infrastructure.Extensions
{
    public static class QueryableUserExtensions
    {
        public static async Task<User> GetUserByIdAsync(this IQueryable<User> users, Guid userId)
        {
            var user = await users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public static async Task<User> GetUserByNameAsync(this IQueryable<User> users,string name)
        {
            var user = await users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public static async Task<User> GetByUserEmailAsync(this IQueryable<User> users,string email)
        {
            var user = await users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

    }
}
