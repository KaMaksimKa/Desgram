using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
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

        public async Task CreateUserAsync(CreateUserDTO createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
        }
    }
}
