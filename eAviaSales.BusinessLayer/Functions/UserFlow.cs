using AutoMapper;
using eAviaSales.BusinessLayer.Core.UserActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Functions
{
    public class UserFlow : UserActions, IUserAction
    {
        // ˜˜˜˜˜˜˜˜˜ ˜˜˜˜˜˜˜˜˜˜˜
        public UserFlow(IMapper mapper) : base(mapper) { }

        public List<UserDto> GetAll() => GetAllUsers();
        public UserDto? GetById(int id) => GetUserById(id);
        public UserDto? Update(int id, UserRegisterDto dto) => UpdateUser(id, dto);
        public UserDto? SetRole(int targetUserId, UserRole role, int callerId) =>
            SetUserRole(targetUserId, role, callerId);
        public void ChangePassword(ClaimsPrincipal principal, ChangePasswordDto dto) => ChangeUserPassword(principal, dto);
        public UserDto? SetActive(int targetUserId, bool isActive, int callerId) =>
            SetUserActive(targetUserId, isActive, callerId);
        public bool Delete(int id) => DeleteUser(id);
    }
}