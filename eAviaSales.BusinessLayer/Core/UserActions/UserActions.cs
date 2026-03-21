using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Core.UserActions
{
    public class UserActions
    {
        private readonly IMapper _mapper;

        public UserActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<UserDto> GetAllUsers()
        {
            using var ctx = new UserContext();
            return _mapper.Map<List<UserDto>>(ctx.Users.OrderBy(u => u.Id).ToList());
        }

        protected UserDto? GetUserById(int id)
        {
            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Id == id && u.IsActive);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        protected UserDto? UpdateUser(int id, UserRegisterDto dto)
        {
            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.FirstName = dto.FirstName;
            user.Username = dto.Username;
            user.Phone = dto.Phone;
            ctx.SaveChanges();
            return _mapper.Map<UserDto>(user);
        }

        protected void ChangeUserPassword(ClaimsPrincipal principal, ChangePasswordDto dto)
        {
            var userId = ExtractUserId(principal);
            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new InvalidOperationException("User not found");

            if (!user.CheckPassword(dto.OldPassword))
                throw new UnauthorizedAccessException("Wrong current password");

            user.SetPasswordHash(dto.NewPassword);
            ctx.SaveChanges();
        }

        protected UserDto? SetUserActive(int targetUserId, bool isActive, int callerId)
        {
            if (targetUserId == callerId)
                throw new InvalidOperationException("You cannot change your own active status.");

            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Id == targetUserId);
            if (user == null) return null;

            user.IsActive = isActive;
            ctx.SaveChanges();
            return _mapper.Map<UserDto>(user);
        }

        protected bool DeleteUser(int id)
        {
            using (var orderCtx = new OrderContext())
            {
                var orders = orderCtx.Orders
                    .Include(o => o.Items)
                    .Where(o => o.UserId == id)
                    .ToList();
                if (orders.Count > 0)
                {
                    orderCtx.Orders.RemoveRange(orders);
                    orderCtx.SaveChanges();
                }
            }

            using var userCtx = new UserContext();
            var user = userCtx.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            userCtx.Users.Remove(user);
            userCtx.SaveChanges();
            return true;
        }

        protected UserDto? SetUserRole(int targetUserId, UserRole newRole, int callerId)
        {
            if (!IsAssignableRole(newRole))
                throw new ArgumentOutOfRangeException(nameof(newRole), "Role must be User, Manager, or Admin.");

            if (targetUserId == callerId)
                throw new InvalidOperationException("You cannot change your own role.");

            using var ctx = new UserContext();
            var user = ctx.Users.FirstOrDefault(u => u.Id == targetUserId);
            if (user == null) return null;

            user.Role = newRole;
            ctx.SaveChanges();
            return _mapper.Map<UserDto>(user);
        }

        private static bool IsAssignableRole(UserRole role) =>
            role is UserRole.User or UserRole.Manager or UserRole.Admin;

        protected static int ExtractUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(value))
                throw new UnauthorizedAccessException("User identity not found in token");
            return int.Parse(value);
        }
    }
}