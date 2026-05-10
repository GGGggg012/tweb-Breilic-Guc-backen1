using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IUserAction
    {
        List<UserDto> GetAll();
        UserDto? GetById(int id);
        UserDto? Update(int id, UserRegisterDto dto);
        UserDto? SetRole(int targetUserId, UserRole role, int callerId);
        void ChangePassword(ClaimsPrincipal principal, ChangePasswordDto dto);
        UserDto? SetActive(int targetUserId, bool isActive, int callerId);
        bool Delete(int id);
    }
}
