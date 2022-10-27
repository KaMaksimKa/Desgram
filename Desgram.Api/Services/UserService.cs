using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Desgram.Api.Config;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Desgram.Api.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly AuthConfig _authConfig;

        public UserService(IMapper mapper,ApplicationContext context,IOptions<AuthConfig> options)
        {
            _mapper = mapper;
            _context = context;
            _authConfig = options.Value;
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



        public async Task<TokenModel> GetToken(string login, string password)
        {
            var user = await GetUserByCredention(login, password);
            var claims = new List<Claim>()
            {
                new Claim("displayName",user.Name),
            };

            var jwt = JwtSecurityToken(
                issuer:_authConfig.Issuer,
                audience:_authConfig.Audience,
                notBefore:DateTime.Now,
                claims:claims,
                expires:DateTime.Now.AddMinutes(_authConfig.LifeTime),
                singingCredentials: new SigningCredentials(_authConfig.Key,SecurityAlgorithms.HmacSha256)
            );



        }

        private async Task<User> GetUserByCredention(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
            {
                throw new Exception("user in not found");
            }

            if (!HashHelper.Verify(login, user.PasswordHash))
            {
                throw new Exception("user in not found");
            }

            return user;
        }
    }
}
