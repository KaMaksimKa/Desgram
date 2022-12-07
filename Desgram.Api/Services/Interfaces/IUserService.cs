using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;
using System.Security.Principal;

namespace Desgram.Api.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Создает временного пользователя и возращает его id, необходимый
        /// для дальнейшего подтверждения пользователя
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public Task<Guid> CreateUserAsync(CreateUserModel createUserDto);
        public Task AddAvatarAsync(MetadataModel model,Guid requestorId);
        /// <summary>
        /// Из временного пользователя создает обычного в случае если код подтверждения верный
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task ConfirmUserAsync(ConfirmUserModel model);
        /// <summary>
        /// Отправляет код подтверждения для временного пользователя
        /// </summary>
        /// <param name="unconfirmedUserId">id временного пользователя</param>
        /// <returns></returns>
        public Task SendSingUpCodeAsync(Guid unconfirmedUserId);
        public Task UpdateProfileAsync(ProfileModel model,Guid requestorId);
        public Task UpdateBirthdayAsync(UpdateBirthdayModel model, Guid requestorId);
        public Task<PersonalInformationModel> GetPersonalInformationAsync(Guid requestorId);
        public Task ChangePasswordAsync(ChangePasswordModel model,Guid requestorId);
        public Task ChangeUserNameAsync(ChangeUserNameModel model, Guid requestorId);
        public Task<Guid> ChangeEmailAsync(ChangeEmailModel model, Guid requestorId);
        public Task ConfirmEmailAsync(ConfirmEmailModel model, Guid requestorId);
        public Task SendChangeEmailCodeAsync(Guid unconfirmedEmailId, Guid requestorId);
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
