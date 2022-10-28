using Desgram.Api.Config;
using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using SharedKernel;
using Microsoft.Extensions.Options;

namespace Desgram.Api.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUserService _userService;
        private readonly AuthConfig _authConfig;

        public AuthService(IUserService userService, IOptions<AuthConfig> options)
        {
            _userService = userService;
            _authConfig = options.Value;
        }

        public async Task<TokenModel> GetTokenByCredentials(string name, string password)
        {
            var user = await _userService.GetUserNameAsync(name);
            if (!VerifyPassword(user, password))
            {
                throw new CustomException("password is not correct");
            }

            var accessToken = GetAccessTokenByUser(user);
            var refreshToken = GetRefreshTokenByUser(user);

            return new TokenModel(accessToken, refreshToken);
        }


        public async Task<TokenModel> GetTokenByRefreshToken(string refreshToken)
        {
            var validationParam = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authConfig.SymmetricSecurityKey
            };

            var principal =
                new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParam, out var securityToken);

            if (securityToken is not JwtSecurityToken jwToken ||
                !jwToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException("token is invalid");
            }

            if (principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value is not {} guidString || 
                !Guid.TryParse(guidString, out var guid))
            {
                throw new CustomException("token is invalid");
            }

            var user = await _userService.GetUserByIdAsync(guid);


            return new TokenModel(GetAccessTokenByUser(user), GetRefreshTokenByUser(user));
        }

        private string GetAccessTokenByUser(User user)
        {
            var access = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                notBefore: DateTime.Now,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Name),
                },
                expires: DateTime.Now.AddMinutes(_authConfig.LifeTime),
                signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedAccess = (new JwtSecurityTokenHandler()).WriteToken(access);

            return encodedAccess;

        }
        private string GetRefreshTokenByUser(User user)
        {
            var refresh = new JwtSecurityToken(
                notBefore: DateTime.Now,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                },
                expires: DateTime.Now.AddHours(_authConfig.LifeTime),
                signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedRefresh = (new JwtSecurityTokenHandler()).WriteToken(refresh);

            return encodedRefresh;

        }

        private bool VerifyPassword(User user, string password)
        {
            return HashHelper.Verify(password, user.PasswordHash);
        }
    }
}
