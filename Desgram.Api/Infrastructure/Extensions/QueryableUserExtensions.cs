using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
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
                throw new CustomException("user not found");
            }

            return user;
        }

        public static async Task<User> GetUserByNameAsync(this IQueryable<User> users,string name)
        {
            var user = await users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        public static async Task<User> GetByUserEmailAsync(this IQueryable<User> users,string email)
        {
            var user = await users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        public static async Task<bool> IsUserExistByEmail(this IQueryable<User> users, string email) =>
            await users.AnyAsync(u=>u.Email.ToLower() == email.ToLower());

        public static async Task<bool> IsUserExistByName(this IQueryable<User> users, string name) =>
            await users.AnyAsync(u => u.Name.ToLower() == name.ToLower());

    }
}
