using Desgram.Api.Models;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenModel> GetTokenByCredentials(string login, string password);
        public Task<TokenModel> GetTokenByRefreshToken(string refreshToken);
        public Task LogoutBySessionId(Guid sessionId);
        public Task LogoutAllDeviceByUserId(Guid userId);
    }
}
