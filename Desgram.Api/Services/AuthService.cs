using Desgram.Api.Config;
using Desgram.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using SharedKernel;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Token;

namespace Desgram.Api.Services
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationContext _context;

        private readonly AuthConfig _authConfig;

        public AuthService(ApplicationContext context, IOptions<AuthConfig> options)
        {
            _context = context;
            _authConfig = options.Value;
        }

        public async Task<TokenModel> GetTokenByCredentialsAsync(string login, string password)
        {

            var user = IsEmail(login) ? await GetByUserEmailAsync(login) : await GetUserByNameAsync(login);

            if (!VerifyPassword(user, password))
            {
                throw new CustomException("password is not correct");
            }

            var session =(await _context.UserSessions.AddAsync(new UserSession()
            {
                Id = Guid.NewGuid(),
                User = user,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                IsActive = true,
                RefreshTokenId = Guid.NewGuid(),
            })).Entity;

            await _context.SaveChangesAsync();

            return new TokenModel{
                AccessToken = GetAccessToken(session),
                RefreshToken = GetRefreshToken(session)

            };
        }

        private bool IsEmail(string str)
        {
            return str.Contains("@");
        }

        public async Task<TokenModel> GetTokenByRefreshTokenAsync(string refreshToken)
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

            var guidString = principal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.RefreshTokenId)?.Value;
            
            if (!Guid.TryParse(guidString, out var guid))
            {
                throw new CustomException("token is invalid");
            }

            var session = await GetSessionWithUserByRefreshAsync(guid);
            if (!session.IsActive)
            {
                throw new CustomException("session is not active");
            }

            session.RefreshTokenId = Guid.NewGuid();
            await _context.SaveChangesAsync();

            return new TokenModel{
                AccessToken = GetAccessToken(session),
                RefreshToken = GetRefreshToken(session)
            };
        }

        public async Task LogoutBySessionIdAsync(Guid sessionId)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.Id == sessionId && s.IsActive);
            if (session == null)
            {
                throw new CustomException("session not found");
            }

            session.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task LogoutAllDeviceByUserIdAsync(Guid userId)
        {
            var user =await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            var sessions = await _context.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }


        private async Task<UserSession> GetSessionWithUserByRefreshAsync(Guid refreshTokenId)
        {
            var session = await _context.UserSessions.Include(s=>s.User)
                .FirstOrDefaultAsync(s => s.RefreshTokenId == refreshTokenId);

            if (session == null)
            {
                throw new CustomException("session not found");
            }

            return session;
        }

        private string GetAccessToken(UserSession session)
        {
            if (session.User == null)
            {
                throw new CustomException("forgot include");
            }

            var access = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                notBefore: DateTime.Now,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,session.User.Id.ToString()),
                    new Claim(AppClaimTypes.SessionId,session.Id.ToString())
                },
                expires: DateTime.Now.AddMinutes(_authConfig.LifeTime),
                signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedAccess = (new JwtSecurityTokenHandler()).WriteToken(access);

            return encodedAccess;

        }

        private string GetRefreshToken(UserSession session)
        {
            var refresh = new JwtSecurityToken(
                notBefore: DateTime.Now,
                claims: new List<Claim>()
                {
                    new Claim(AppClaimTypes.RefreshTokenId,session.RefreshTokenId.ToString())
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

        private async Task<User> GetUserByNameAsync(string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }

        private async Task<User> GetByUserEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }
    }
}
