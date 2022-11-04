using Desgram.Api.Models;

namespace Desgram.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenModel> GetTokenByCredentials(string login, string password);
        Task<TokenModel> GetTokenByRefreshToken(string refreshToken);
    }
}
