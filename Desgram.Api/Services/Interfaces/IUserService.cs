using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using System.Security.Principal;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> CreateUserAsync(CreateUserModel createUserDto);
        public Task AddAvatarAsync(MetadataModel model,Guid userId);
        public Task ConfirmUserAsync(string code,Guid unconfirmedUserId);
        public Task SendSingUpCodeAsync(Guid unconfirmedUserId);
        public Task UpdateProfileAsync(ProfileModel model,Guid userId);
        public Task ChangePasswordAsync(ChangePasswordModel model,Guid userId);
        public Task ChangeUserNameAsync(ChangeUserNameModel model, Guid userId);
        public Task<Guid> ChangeEmailAsync(ChangeEmailModel model, Guid userId);
        public Task ConfirmEmailAsync(string code, Guid unconfirmedEmailId, Guid userId);
        public Task SendChangeEmailCodeAsync(Guid unconfirmedEmailId, Guid userId);
        public Task ChangeAccountAvailabilityAsync(bool isPrivate, Guid userId);
        public Task<UserModel> GetUserByIdAsync(Guid userId);
        public Task<List<PartialUserModel>> SearchUsersByNameAsync(SearchUsersByNameModel model, Guid userId);

    }
}
