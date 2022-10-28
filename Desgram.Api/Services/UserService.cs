using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public UserService(IMapper mapper,ApplicationContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateUserAsync(CreateUserModel createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => _mapper.Map<UserModel>(u)).ToList();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user =await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }

        public async Task<User> GetUserNameAsync(string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }


    }
}
