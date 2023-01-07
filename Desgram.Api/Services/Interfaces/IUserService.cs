using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using System.Security.Principal;
using Desgram.Api.Models;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        public Task TryCreateUserAsync(TryCreateUserModel createUser);
        public Task<GuidIdModel> SendSingUpCodeAsync(string email);
        public Task CreateUserAsync(CreateUserModel model);
        public Task AddAvatarAsync(MetadataModel model,Guid requestorId);
        public Task DeleteAvatarAsync(Guid requestorId);
        public Task UpdateProfileAsync(ProfileModel model,Guid requestorId);
        public Task UpdateBirthdayAsync(UpdateBirthdayModel model, Guid requestorId);
        public Task<PersonalInformationModel> GetPersonalInformationAsync(Guid requestorId);
        public Task ChangePasswordAsync(ChangePasswordModel model,Guid requestorId);
        public Task ChangeUserNameAsync(ChangeUserNameModel model, Guid requestorId);
        public Task ChangeEmailAsync(ChangeEmailModel model, Guid requestorId);
        public Task TryChangeEmailAsync(string newEmail, Guid requestorId);
        public Task<GuidIdModel> SendChangeEmailCodeAsync(string email, Guid requestorId);
        /// <summary>
        /// Изменить приватность аккаунта, если isPrivate равно true,то приватный, если false, то нет
        /// </summary>
        /// <param name="isPrivate"></param>
        /// <param name="requestorId"></param>
        /// <returns></returns>
        public Task ChangeAccountAvailabilityAsync(bool isPrivate, Guid requestorId);
        public Task<UserModel> GetUserByIdAsync(Guid userId,Guid requestorId);
        public Task<List<PartialUserModel>> SearchUsersByNameAsync(SearchUsersByNameModel model, Guid requestorId);

    }
}
