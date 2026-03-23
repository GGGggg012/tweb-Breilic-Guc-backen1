using eAviaSales.BusinessLayer.Structure;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Core.UserActions
{
    public class UserAuthActions
    {
        protected LoginResultDto Login(UserLoginDto dto, string jwtKey, string issuer, string audience)
        {
            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !user.CheckPassword(dto.Password))
                throw new UnauthorizedAccessException("Invalid email or password");
            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is disabled.");

            var tokenService = new TokenService(jwtKey, issuer, audience);
            var token = tokenService.GenerateToken(user.Id, user.Username, user.Role.ToString());

            return new LoginResultDto
            {
                Token = token,
                Role = user.Role.ToString(),
                UserId = user.Id,
                Username = user.Username
            };
        }

        protected void Register(RegisterAccountDto dto, string? configuredBootstrapSecret)
        {
            using var ctx = new UserContext();
            if (ctx.Users.Any(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email already in use");

            var user = new UserData
            {
                FirstName = dto.FirstName,
                Username = dto.Username,
                Email = dto.Email,
                Phone = dto.Phone,
                RegisteredOn = DateTime.UtcNow,
                Role = ResolveRegistrationRole(dto.BootstrapSecret, configuredBootstrapSecret, ctx),
                IsActive = true
            };
            user.SetPasswordHash(dto.Password);
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        private static UserRole ResolveRegistrationRole(
            string? providedBootstrapSecret,
            string? configuredBootstrapSecret,
            UserContext ctx)
        {
            if (string.IsNullOrWhiteSpace(providedBootstrapSecret)
                || string.IsNullOrWhiteSpace(configuredBootstrapSecret))
            {
                return UserRole.User;
            }

            if (ctx.Users.Any(u => u.IsActive && u.Role == UserRole.Admin))
                return UserRole.User;

            return string.Equals(
                providedBootstrapSecret.Trim(),
                configuredBootstrapSecret.Trim(),
                StringComparison.Ordinal)
                ? UserRole.Admin
                : UserRole.User;
        }
    }
}
