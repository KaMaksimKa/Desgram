using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> CreateUserAsync(CreateUserModel createUserDto);
        public Task<List<UserModel>> GetUsersAsync();
        public Task<UserModel> GetUserByIdAsync(Guid userId);
        public Task AddAvatarAsync(MetadataModel model,Guid userId);
        public Task ConfirmEmailByCodeAsync(string code,Guid unconfirmedUserId);
        public Task SendEmailConfirmationCodeAsync(Guid unconfirmedUserId);
        public Task UpdateProfileAsync(ProfileModel model,Guid userId);
        public Task ChangePasswordAsync(ChangePasswordModel model,Guid userId);
        public Task ChangeEmailAsync(ChangeEmailModel model,Guid userId);
        public Task ChangeUserName(string newUserName,Guid userId);

    }
}
