using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.EmailMessage;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Desgram.Api.Services
{
    public class UserService:IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly IAttachService _attachService;
        private readonly IEmailSender _emailSender;
        private readonly IUrlService _urlService;


        public UserService(IMapper mapper,ApplicationContext context,
            IAttachService attachService,IEmailSender emailSender,IUrlService urlService)
        {
            _mapper = mapper;
            _context = context;
            _attachService = attachService;
            _emailSender = emailSender;
            _urlService = urlService;
        }

        public async Task<Guid> CreateUserAsync(CreateUserModel createUser)
        {

            if (await _context.Users.IsUserExistByName(createUser.Name))
            {
                throw new CustomException($"username is busy");
            }

            if (await _context.Users.IsUserExistByEmail(createUser.Email))
            {
                throw new CustomException($"email is busy");
            }

            var code = CodeGenerator.GetCode(6);

            var unconfirmedUser = new UnconfirmedUser
            {
                Id = Guid.NewGuid(),
                CodeHash = HashHelper.GetHash(code),
                ExpiredDate = DateTimeOffset.Now.UtcDateTime.AddMinutes(5),
                Email = createUser.Email,
                Name = createUser.Name,
                PasswordHash = HashHelper.GetHash(createUser.Password)
            };

            await _context.UnconfirmedUsers.AddAsync(unconfirmedUser);
            await _context.SaveChangesAsync();

            await SendSingUpCodeAsync(unconfirmedUser.Id);

            return unconfirmedUser.Id;
        }

        public async Task ConfirmUserAsync(string code, Guid unconfirmedUserId)
        {
            var unconfirmedUser = await GetUnconfirmedUserById(unconfirmedUserId);

            if (DateTimeOffset.Now.UtcDateTime > unconfirmedUser.ExpiredDate)
            {
                throw new CustomException("code has expired");
            }

            if (!HashHelper.Verify(code, unconfirmedUser.CodeHash))
            {
                throw new CustomException("invalid code");
            }

            unconfirmedUser.DeletedDate = DateTimeOffset.Now.UtcDateTime;


            if (await _context.Users.IsUserExistByName(unconfirmedUser.Name))
            {
                throw new CustomException($"username is busy");
            }

            if (await _context.Users.IsUserExistByEmail(unconfirmedUser.Email))
            {
                throw new CustomException($"email is busy");
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = unconfirmedUser.Name,
                Email = unconfirmedUser.Email,
                PasswordHash = unconfirmedUser.PasswordHash,
                CreatedDate = DateTimeOffset.Now.UtcDateTime
            };


            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task SendSingUpCodeAsync(Guid unconfirmedUserId)
        {
            var unconfirmedUser = await GetUnconfirmedUserById(unconfirmedUserId);

            if (DateTimeOffset.Now.UtcDateTime > unconfirmedUser.ExpiredDate)
            {
                throw new CustomException("code has expired");
            }

            var code = CodeGenerator.GetCode(6);

            await _emailSender.SendEmailAsync(new SingUpCodeMessage(unconfirmedUser.Email, code));

            unconfirmedUser.CodeHash = HashHelper.GetHash(code);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(ProfileModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            user.BirthDate = model.BirthDate;
            user.Biography = model.Biography;
            user.FullName = model.FullName;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            if (!HashHelper.Verify(model.OldPassword, user.PasswordHash))
            {
                throw new CustomException("password is not correct");
            }

            user.PasswordHash = model.NewPassword;

            await _context.SaveChangesAsync();

        }

        public async Task ChangeUserNameAsync(ChangeUserNameModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            if (await _context.Users.IsUserExistByName(model.NewName))
            {
                throw new CustomException("name is busy");
            }

            user.Name = model.NewName;

            await _context.SaveChangesAsync();
        }

        public async Task<Guid> ChangeEmailAsync(ChangeEmailModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            if (await _context.Users.IsUserExistByEmail(model.NewEmail))
            {
                throw new CustomException("email is busy");
            }

            var code = CodeGenerator.GetCode(6);

            var unconfirmedEmail = new UnconfirmedEmail()
            {
                Id = Guid.NewGuid(),
                CodeHash = HashHelper.GetHash(code),
                Email = model.NewEmail,
                DeletedDate = null,
                ExpiredDate = DateTimeOffset.Now.UtcDateTime.AddMinutes(5),
                User = user
            };

            await _context.UnconfirmedEmails.AddAsync(unconfirmedEmail);
            await _context.SaveChangesAsync();

            await SendChangeEmailCodeAsync(unconfirmedEmail.Id, userId);

            return unconfirmedEmail.Id;
        }

        public async Task ConfirmEmailAsync(string code, Guid unconfirmedEmailId, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            var unconfirmedEmail = await GetUnconfirmedEmailById(unconfirmedEmailId, userId);

            if (DateTimeOffset.Now.UtcDateTime > unconfirmedEmail.ExpiredDate)
            {
                throw new CustomException("code has expired");
            }

            if (!HashHelper.Verify(code, unconfirmedEmail.CodeHash))
            {
                throw new CustomException("invalid code");
            }

            unconfirmedEmail.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            if (await _context.Users.IsUserExistByEmail(unconfirmedEmail.Email))
            {
                throw new CustomException($"email is busy");
            }

            user.Email = unconfirmedEmail.Email;

            await _context.SaveChangesAsync();

        }

        public async Task SendChangeEmailCodeAsync(Guid unconfirmedEmailId, Guid userId)
        {
            var unconfirmedEmail = await GetUnconfirmedEmailById(unconfirmedEmailId, userId);

            if (unconfirmedEmail.ExpiredDate < DateTimeOffset.Now.UtcDateTime)
            {
                throw new CustomException("code has expired");
            }

            var code = CodeGenerator.GetCode(6);

            await _emailSender.SendEmailAsync(new ChangeEmailCodeMessage(unconfirmedEmail.Email, code));

            unconfirmedEmail.CodeHash = HashHelper.GetHash(code);

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var users = await _context.Users
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var user in users)
            {
                if (user.Avatar != null)
                {
                    user.Avatar.Url = _urlService.GetUrlDisplayAttachById(user.Avatar.Id);
                }
            }
            return users;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Where(u=>u.Id == userId)
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new CustomException("user not found");
            }

            if (user.Avatar != null)
            {
                user.Avatar.Url = _urlService.GetUrlDisplayAttachById(user.Avatar.Id);
            }
          
            return user;
        }

        public async Task AddAvatarAsync(MetadataModel model,Guid userId)
        {
            var pathAttach = _attachService.MoveFromTempToAttach(model);

            var user = await GetUserWithAvatarsByIdAsync(userId);

            var avatar = new Avatar()
            {
                User = user,
                Id = Guid.NewGuid(),
                Name = model.Name,
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                MimeType = model.MimeType,  
                Owner = user,
                Path = pathAttach,
                Size = model.Size,
                DeletedDate = null
            };

            if (user.Avatars.FirstOrDefault(a=>a.DeletedDate==null) is {} oldAvatar)
            {
                oldAvatar.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            }
            
            await _context.Avatars.AddAsync(avatar);
            await _context.SaveChangesAsync();
        }

        private async Task<User> GetUserWithAvatarsByIdAsync(Guid id)
        {

            var user = await _context.Users.Include(u=>u.Avatars).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }

        private async Task<UnconfirmedEmail> GetUnconfirmedEmailById(Guid unconfirmedEmailId, Guid userId)
        {
            var unconfirmedEmail = await _context.UnconfirmedEmails
                .FirstOrDefaultAsync(e => e.Id == unconfirmedEmailId
                                          && e.DeletedDate == null && e.UserId == userId);

            if (unconfirmedEmail == null)
            {
                throw new CustomException("unconfirmedEmail not found");
            }
            return unconfirmedEmail;
        }

        private async Task<UnconfirmedUser> GetUnconfirmedUserById(Guid unconfirmedUserId)
        {
            var unconfirmedUser = await _context.UnconfirmedUsers
                .FirstOrDefaultAsync(u => u.Id == unconfirmedUserId && u.DeletedDate == null);

            if (unconfirmedUser == null)
            {
                throw new CustomException("unconfirmedUser not found");
            }
            return unconfirmedUser;
        }

    }
}
