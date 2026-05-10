using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IUserLoginAction
    {
        LoginResultDto Login(UserLoginDto dto);
    }
}
