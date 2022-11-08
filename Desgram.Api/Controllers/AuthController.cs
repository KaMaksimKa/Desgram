﻿using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Token;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
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
            return await _authService.GetTokenByCredentials(model.UserName, model.Password);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
        {
            return await _authService.GetTokenByRefreshToken(model.RefreshToken);
        }


        [HttpPost]
        public async Task Logout()
        {
            var sessionId = User.GetSessionId();
            await _authService.LogoutBySessionId(sessionId);
        }

        [HttpPost]
        public async Task LogoutAllDevice()
        {
            var userId = User.GetUserId();
            await _authService.LogoutAllDeviceByUserId(userId);
        }
    }
}
