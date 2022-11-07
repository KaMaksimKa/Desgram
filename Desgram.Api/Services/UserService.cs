using AutoMapper;
using Desgram.Api.Models;
using Desgram.Api.Services.Dto;
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
        private readonly IAttachService _attachService;
        private readonly IEmailSender _emailSender;


        public UserService(IMapper mapper,ApplicationContext context,
            IAttachService attachService,IEmailSender emailSender)
        {
            _mapper = mapper;
            _context = context;
            _attachService = attachService;
            _emailSender = emailSender;
        }

        public async Task CreateUserAsync(CreateUserModel createUser)
        {
            var user = _mapper.Map<User>(createUser);

            await CreateAsync(user);
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = (await GetAll()).ToList();
            return users.Select(u => _mapper.Map<UserModel>(u)).ToList(); 
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);

            return _mapper.Map<UserModel>(user);
        }

        public async Task AddAvatarAsync(MetadataModel model,Guid userId)
        {
            var pathAttach = _attachService.MoveFromTempToAttach(model);

            var user = await GetByIdAsync(userId);

            var avatar = new Avatar()
            {
                User = user,
                Id = Guid.NewGuid(),
                Name = model.Name,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                MimeType = model.MimeType,  
                Owner = user,
                Path = pathAttach
            };

            await _context.Avatars.AddAsync(avatar);
            await _context.SaveChangesAsync();
        }

        public async Task<AttachModel> GetAvatarAsync(Guid userId)
        {
            var avatar =(await GetUserWithAvatarByIdAsync(userId)).Avatar;
            if (avatar == null)
            {
                throw new CustomException("avatar is not exist");
            }

            return new AttachModel()
            {
                FilePath = avatar.Path,
                MimeType = avatar.MimeType,
                Name = avatar.Name
            };
        }

        private async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        private async Task<User> GetByNameAsync(string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        private async Task<User> GetByIdAsync(Guid id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }
        private async Task<User> GetUserWithAvatarByIdAsync(Guid id)
        {

            var user = await _context.Users.Include(u=>u.Avatar).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }
        private async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        private async Task CreateAsync(User user)
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
