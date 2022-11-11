using Desgram.Api.Models.Token;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenModel> GetTokenByCredentialsAsync(string login, string password);
        public Task<TokenModel> GetTokenByRefreshTokenAsync(string refreshToken);
        public Task LogoutBySessionIdAsync(Guid sessionId);
        public Task LogoutAllDeviceByUserIdAsync(Guid userId);
    }
}
