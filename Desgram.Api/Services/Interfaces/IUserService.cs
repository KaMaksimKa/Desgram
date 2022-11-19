using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using System.Security.Principal;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> CreateUserAsync(CreateUserModel createUserDto);
        public Task AddAvatarAsync(MetadataModel model,Guid requestorId);
        public Task ConfirmUserAsync(ConfirmUserModel model);
        public Task SendSingUpCodeAsync(Guid unconfirmedUserId);
        public Task UpdateProfileAsync(ProfileModel model,Guid requestorId);
        public Task ChangePasswordAsync(ChangePasswordModel model,Guid requestorId);
        public Task ChangeUserNameAsync(ChangeUserNameModel model, Guid requestorId);
        public Task<Guid> ChangeEmailAsync(ChangeEmailModel model, Guid requestorId);
        public Task ConfirmEmailAsync(ConfirmEmailModel model, Guid requestorId);
        public Task SendChangeEmailCodeAsync(Guid unconfirmedEmailId, Guid requestorId);
        public Task ChangeAccountAvailabilityAsync(bool isPrivate, Guid requestorId);
        public Task<UserModel> GetUserByIdAsync(Guid userId,Guid requestorId);
        public Task<List<PartialUserModel>> SearchUsersByNameAsync(SearchUsersByNameModel model, Guid requestorId);

    }
}
