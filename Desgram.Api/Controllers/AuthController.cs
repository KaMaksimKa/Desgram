using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Token;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Auth")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenModel> Token(TokenRequestModel model)
        {
            return await _authService.GetTokenByCredentialsAsync(model.Login, model.Password);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
        {
            return await _authService.GetTokenByRefreshTokenAsync(model.RefreshToken);
        }

        [ApiExplorerSettings(GroupName = "Api")]
        [HttpPost]
        public async Task Logout()
        {
            var sessionId = User.GetSessionId();
            await _authService.LogoutBySessionIdAsync(sessionId);
        }

        [ApiExplorerSettings(GroupName = "Api")]
        [HttpPost]
        public async Task LogoutAllDevice()
        {
            var userId = User.GetUserId();
            await _authService.LogoutAllDeviceByUserIdAsync(userId);
        }

        [ApiExplorerSettings(GroupName = "Admin")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task LogoutAllUsers()
        {
            await _authService.LogoutAllUsersAsync();
        }
    }
}
