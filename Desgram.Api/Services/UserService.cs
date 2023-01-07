using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
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
        private readonly IConfirmService _confirmService;
        private readonly ICustomMapperService _customMapperService;


        public UserService(IMapper mapper,ApplicationContext context,
            IAttachService attachService,IConfirmService confirmService,ICustomMapperService customMapperService)
        {
            _mapper = mapper;
            _context = context;
            _attachService = attachService;
            _confirmService = confirmService;
            _customMapperService = customMapperService;
        }

        public async Task TryCreateUserAsync(TryCreateUserModel createUser)
        {
            BadRequestException badRequestException = new BadRequestException();
            if (!(await IsUserNameFree(createUser.UserName)))
            {
                badRequestException.Errors.Add(nameof(createUser.UserName),new List<string>()
                {
                    $"Имя пользователя {createUser.UserName} занято."
                });
            }

            if (!(await IsEmailFree(createUser.Email)))
            {
                badRequestException.Errors.Add(nameof(createUser.UserName), new List<string>()
                {
                    $"email {createUser.Email} занят."
                });

            }

            if (badRequestException.Errors.Count != 0)
            {
                throw badRequestException;
            }
        }

        public async Task<GuidIdModel> SendSingUpCodeAsync(string email)
        {
            return await _confirmService.SendEmailCodeAsync(email, TypesEmailCodeMessage.SingUpMessage);
        }

        public async Task CreateUserAsync(CreateUserModel model)
        {
            await TryCreateUserAsync(model.TryCreateUserModel);
            await _confirmService.ConfirmEmailCodeAsync(model.EmailCodeModel);

            await _context.Users.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = model.TryCreateUserModel.UserName,
                Email = model.TryCreateUserModel.Email,
                PasswordHash = HashHelper.GetHash(model.TryCreateUserModel.Password),
                CreatedDate = DateTimeOffset.Now.UtcDateTime
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAvatarAsync(Guid requestorId)
        {

            var user = await GetUserWithAvatarsByIdAsync(requestorId);

            if (user.Avatars.FirstOrDefault(a => a.DeletedDate == null) is { } oldAvatar)
            {
                oldAvatar.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            }
            else
            {
                throw new AvatarNotFoundException();
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(ProfileModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            user.Biography = model.Biography?? user.Biography;
            user.FullName = model.FullName?? user.FullName;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBirthdayAsync(UpdateBirthdayModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (model.Birthday != null)
            {
                user.BirthDate = model.Birthday.Value;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PersonalInformationModel> GetPersonalInformationAsync(Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            return new PersonalInformationModel()
            {
                BirthDate = user.BirthDate,
                Email = user.Email,
            };
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (!HashHelper.Verify(model.OldPassword, user.PasswordHash))
            {
                throw new BadRequestException()
                {
                    Errors =
                    {
                        [nameof(model.OldPassword)] = new List<string>()
                        {
                            "Неверный пароль"
                        }
                    }
                };
            }

            user.PasswordHash = HashHelper.GetHash(model.NewPassword);

            await _context.SaveChangesAsync();
        }

        public async Task ChangeUserNameAsync(ChangeUserNameModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            if (user.Name.ToLower() == model.NewName.ToLower())
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(model.NewName)] = new List<string>()
                    {
                        "Нельзя сменить имя пользователя на текущие"
                    }
                });
            }

            if (!(await IsUserNameFree(model.NewName)))
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(model.NewName)] = new List<string>()
                    {
                        $"Имя пользователя {model.NewName} занято."
                    }
                });
            }
          


            user.Name = model.NewName;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeEmailAsync(ChangeEmailModel model, Guid requestorId)
        {
            await TryChangeEmailAsync(model.NewEmail,requestorId);
            await _confirmService.ConfirmEmailCodeAsync(model.EmailCodeModel);
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            user.Email = model.NewEmail;

            await _context.SaveChangesAsync();
        }

        public async Task TryChangeEmailAsync(string newEmail, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            if (user.Email == newEmail)
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(newEmail)] = new List<string>()
                    {
                        "Нельзя сменить e-mail на текущий"
                    }
                });
            }

            if (!(await IsEmailFree(newEmail)))
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(newEmail)] = new List<string>()
                    {
                        $"Email {newEmail} занят."
                    }
                });
            }
        }

        public async Task<GuidIdModel> SendChangeEmailCodeAsync(string email, Guid requestorId)
        {
            return await _confirmService.SendEmailCodeAsync(email, TypesEmailCodeMessage.ChangeEmailMessage);
        }

        public async Task ChangeAccountAvailabilityAsync(bool isPrivate, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);
            
            user.IsPrivate = isPrivate;

            await _context.SaveChangesAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId,Guid requestorId)
        {
            var users = _context.Users.AsNoTracking()
                .Where(u => u.Id == userId);

            var user = (await _customMapperService.ToUserModelsList(users, requestorId)).FirstOrDefault();

            if (user == null)
            {
                throw new UserNotFoundException();
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

        private async Task<bool> IsEmailFree(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        private async Task<bool> IsUserNameFree(string username)
        {
            return !await _context.Users.AnyAsync(u => u.Name.ToLower() == username.ToLower());
        }


    }
}
