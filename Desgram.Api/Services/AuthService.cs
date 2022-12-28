using Desgram.Api.Config;
using Desgram.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Desgram.Api.Infrastructure;
using Desgram.Api.Models.Token;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.SharedKernel;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Desgram.SharedKernel.Exceptions.UnauthorizedExceptions;


namespace Desgram.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;
        private readonly IRoleService _roleService;

        private readonly AuthConfig _authConfig;

        public AuthService(ApplicationContext context, IOptions<AuthConfig> options,
            IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
            _authConfig = options.Value;
        }

        public async Task<TokenModel> GetTokenByCredentialsAsync(string login, string password)
        {

            var user = IsEmail(login) ? 
                await _context.Users.GetByUserEmailAsync(login) 
                : await _context.Users.GetUserByNameAsync(login);

            if (!HashHelper.Verify(password, user.PasswordHash))
            {
                throw new InvalidPasswordException();
            }

            var session = (await _context.UserSessions.AddAsync(new UserSession()
            {
                Id = Guid.NewGuid(),
                User = user,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                IsActive = true,
                RefreshTokenId = Guid.NewGuid(),
            })).Entity;

            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = await GetAccessTokenAsync(session),
                RefreshToken = GetRefreshToken(session)

            };
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

            ClaimsPrincipal principal;
            SecurityToken securityToken;
            try
            {
                principal =
                    new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParam, out securityToken);
            }
            catch 
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
                var refreshIdStr = jwtToken.Claims.FirstOrDefault(c=>c.Type == AppClaimTypes.RefreshTokenId)?.Value;
                if (Guid.TryParse(refreshIdStr, out var refreshId))
                {
                    var userSession = await GetSessionByRefreshIdAsync(refreshId);
                    userSession.PushToken = null;
                    await _context.SaveChangesAsync();
                }
                throw new InvalidRefreshTokenException();
            }
            

            if (securityToken is not JwtSecurityToken jwToken ||
                !jwToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidRefreshTokenException();
            }

            var refreshTokenId = principal.GetRefreshTokenId();

            var session = await GetSessionByRefreshIdAsync(refreshTokenId);
            if (!session.IsActive)
            {
                throw new SessionIsNotActiveException();
            }

            session.RefreshTokenId = Guid.NewGuid();
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken =await GetAccessTokenAsync(session),
                RefreshToken = GetRefreshToken(session)
            };
        }

        public async Task LogoutBySessionIdAsync(Guid sessionId)
        {
            var session = await _context.UserSessions.GetSessionById(sessionId);

            session.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task LogoutAllDeviceByUserIdAsync(Guid userId)
        {
       
            var sessions = await _context.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();


            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task LogoutAllUsersAsync()
        {
            var sessions = await _context.UserSessions
                .Where(s => s.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<string> GetAccessTokenAsync(UserSession session)
        {
            

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                new Claim(AppClaimTypes.SessionId, session.Id.ToString()),
            };

            var roles = await _roleService.GetUserRoles(session.UserId);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var access = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                notBefore: DateTime.Now,
                claims:claims,
                expires: DateTime.Now.AddMinutes(_authConfig.LifeTime),
                signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256)
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
                    new Claim(AppClaimTypes.RefreshTokenId, session.RefreshTokenId.ToString())
                },
                expires: DateTime.Now.AddHours(_authConfig.LifeTime),
                signingCredentials: new SigningCredentials(_authConfig.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256)
            );

            var encodedRefresh = (new JwtSecurityTokenHandler()).WriteToken(refresh);

            return encodedRefresh;

        }

        private async Task<UserSession> GetSessionByRefreshIdAsync(Guid refreshTokenId)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.RefreshTokenId == refreshTokenId);

            if (session == null)
            {
                throw new SessionNotFoundException();
            }

            return session;
        }

        

        private bool IsEmail(string str)
        {
            return str.Contains("@");
        }
    }
}
