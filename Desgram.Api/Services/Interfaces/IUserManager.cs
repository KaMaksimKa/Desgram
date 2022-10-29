using Desgram.DAL.Entities;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserManager
    {
        public Task<User> GetByEmailAsync(string email);
        public Task<User> GetByNameAsync(string name);
        public Task<User> GetByIdAsync(Guid id);
        public Task<IEnumerable<User>> GetAll();
        public Task CreateAsync(User user);

    }
}
