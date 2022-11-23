using Desgram.Api.Models.Token;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenModel> GetTokenByCredentialsAsync(string login, string password);
        public Task<TokenModel> GetTokenByRefreshTokenAsync(string refreshToken);
        /// <summary>
        /// Делает сессию неактивной
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public Task LogoutBySessionIdAsync(Guid sessionId);
        /// <summary>
        /// Делает все сессии у пользователя неактивными
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task LogoutAllDeviceByUserIdAsync(Guid userId);
        /// <summary>
        /// Делает абсолютно все сессии неактивными
        /// </summary>
        /// <returns></returns>
        public Task LogoutAllUsersAsync();
    }
}
