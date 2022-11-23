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
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
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

        public async Task<Guid> CreateUserAsync(CreateUserModel createUser)
        {

            if (!(await IsUserNameFree(createUser.UserName)))
            {
                throw new UserNameIsBusyException();
            }

            if (!(await IsEmailFree(createUser.Email)))
            {
                throw new EmailIsBusyException();
            }

            var code = CodeGenerator.GetCode(6);

            var unconfirmedUser = new UnconfirmedUser
            {
                Id = Guid.NewGuid(),
                CodeHash = HashHelper.GetHash(code),
                ExpiredDate = DateTimeOffset.Now.UtcDateTime.AddMinutes(5),
                Email = createUser.Email,
                Name = createUser.UserName,
                PasswordHash = HashHelper.GetHash(createUser.Password)
            };

            await _context.UnconfirmedUsers.AddAsync(unconfirmedUser);
            await _context.SaveChangesAsync();

            return unconfirmedUser.Id;
        }

        public async Task ConfirmUserAsync(ConfirmUserModel model)
        {
            var unconfirmedUser = await GetUnconfirmedUserById(model.UnconfirmedUserId);

            if (DateTimeOffset.Now.UtcDateTime > unconfirmedUser.ExpiredDate)
            {
                throw new ConfirmCodeHasExpiredException();
            }

            if (!HashHelper.Verify(model.ConfirmCode, unconfirmedUser.CodeHash))
            {
                throw new InvalidConfirmCodeException();
            }

            unconfirmedUser.DeletedDate = DateTimeOffset.Now.UtcDateTime;

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
                throw new ConfirmCodeHasExpiredException();
            }

            var code = CodeGenerator.GetCode(6);

            await _emailSender.SendEmailAsync(new SingUpCodeMessage(unconfirmedUser.Email, code));

            unconfirmedUser.CodeHash = HashHelper.GetHash(code);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(ProfileModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            user.BirthDate = model.BirthDate;
            user.Biography = model.Biography;
            user.FullName = model.FullName;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (!HashHelper.Verify(model.OldPassword, user.PasswordHash))
            {
                throw new InvalidPasswordException();
            }

            user.PasswordHash = HashHelper.GetHash(model.NewPassword);

            await _context.SaveChangesAsync();

        }

        public async Task ChangeUserNameAsync(ChangeUserNameModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (!(await IsUserNameFree(model.NewName)))
            {
                throw new UserNameIsBusyException();
            }

            user.Name = model.NewName;

            await _context.SaveChangesAsync();
        }

        public async Task<Guid> ChangeEmailAsync(ChangeEmailModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (!(await IsEmailFree(model.NewEmail)))
            {
                throw new EmailIsBusyException();
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

            await SendChangeEmailCodeAsync(unconfirmedEmail.Id, requestorId);

            return unconfirmedEmail.Id;
        }

        public async Task ConfirmEmailAsync(ConfirmEmailModel model, Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            var unconfirmedEmail = await GetUnconfirmedEmailById(model.UnconfirmedEmailId, userId);

            if (DateTimeOffset.Now.UtcDateTime > unconfirmedEmail.ExpiredDate)
            {
                throw new ConfirmCodeHasExpiredException();
            }

            if (!HashHelper.Verify(model.ConfirmCode, unconfirmedEmail.CodeHash))
            {
                throw new InvalidConfirmCodeException();
            }

            unconfirmedEmail.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            user.Email = unconfirmedEmail.Email;

            await _context.SaveChangesAsync();

        }

        public async Task SendChangeEmailCodeAsync(Guid unconfirmedEmailId, Guid requestorId)
        {
            var unconfirmedEmail = await GetUnconfirmedEmailById(unconfirmedEmailId, requestorId);

            if (unconfirmedEmail.ExpiredDate < DateTimeOffset.Now.UtcDateTime)
            {
                throw new ConfirmCodeHasExpiredException();
            }

            var code = CodeGenerator.GetCode(6);

            await _emailSender.SendEmailAsync(new ChangeEmailCodeMessage(unconfirmedEmail.Email, code));

            unconfirmedEmail.CodeHash = HashHelper.GetHash(code);

            await _context.SaveChangesAsync();
        }

        public async Task ChangeAccountAvailabilityAsync(bool isPrivate, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            
            user.IsPrivate = isPrivate;

            await _context.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId,Guid requestorId)
        {

            var user = await _context.Users.AsNoTracking()
                .Where(u => u.Id == userId)
                .ProjectToByRequestorId<UserModel>(_mapper.ConfigurationProvider,requestorId)
                .FirstOrDefaultAsync();

            
            if (user == null)
            {
                throw new UserNotFoundException();
            }


            if (user.HasBlockedViewer)
            {
                throw new AccessActionException();
            }


            return _mapper.Map<UserModel>(user);
        }

        public async Task<List<PartialUserModel>> SearchUsersByNameAsync(SearchUsersByNameModel model, Guid requestorId)
        {
            var users = await _context.Users.AsNoTracking()
                .Where(u => u.Name.ToLower().Contains(model.SearchUserName.ToLower())
                            && !u.BlockedUsers.Any(ub => ub.BlockedId == requestorId && ub.DeletedDate == null))
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return users.Select(u=>_mapper.Map<PartialUserModel>(u)).ToList();
        }

        public async Task AddAvatarAsync(MetadataModel model,Guid requestorId)
        {


            var user = await GetUserWithAvatarsByIdAsync(requestorId);


            var avatar = new Avatar()
            {
                User = user,
                Id = Guid.NewGuid(),
                ImageCandidates  = (await _attachService.FromTempToImage(model))
                .Select(c => new Image()
                {
                    CreatedDate = DateTimeOffset.Now.UtcDateTime,
                    DeletedDate = null,
                    Name = model.Name,
                    MimeType = c.MimeType,
                    Id = c.Id,
                    Path = c.Path,
                    Owner = user,
                    Height = c.Height,
                    Width = c.Width,
                })
                .ToList(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null,
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
                throw new UserNotFoundException();
            }

            return user;
        }

        private async Task<UnconfirmedEmail> GetUnconfirmedEmailById(Guid unconfirmedEmailId, Guid requestorId)
        {
            var unconfirmedEmail = await _context.UnconfirmedEmails
                .FirstOrDefaultAsync(e => e.Id == unconfirmedEmailId
                                          && e.DeletedDate == null && e.UserId == requestorId);

            if (unconfirmedEmail == null)
            {
                throw new UnconfirmedEmailNotFoundException();
            }
            return unconfirmedEmail;
        }

        private async Task<UnconfirmedUser> GetUnconfirmedUserById(Guid unconfirmedUserId)
        {
            var unconfirmedUser = await _context.UnconfirmedUsers
                .FirstOrDefaultAsync(u => u.Id == unconfirmedUserId && u.DeletedDate == null);

            if (unconfirmedUser == null)
            {
                throw new UnconfirmedUserNotFoundException();
            }
            return unconfirmedUser;
        }

        private async Task<bool> IsEmailFree(string email)
        {
            return !(await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                   && !(await _context.UnconfirmedUsers.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                   && !(await _context.UnconfirmedEmails.AnyAsync(e => e.Email.ToLower() == email.ToLower()));
        }

        private async Task<bool> IsUserNameFree(string username)
        {
            return !(await _context.Users.AnyAsync(u => u.Name.ToLower() == username.ToLower()))
                   && !(await _context.UnconfirmedUsers.AnyAsync(u => u.Name.ToLower() == username.ToLower()));
        }


    }
}
