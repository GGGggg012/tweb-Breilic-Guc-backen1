using eAviaSales.BusinessLayer.Core.UserActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Functions
{
    public class UserLoginFlow : UserAuthActions, IUserLoginAction
    {
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public UserLoginFlow(string jwtKey, string issuer, string audience)
        {
            _jwtKey = jwtKey;
            _issuer = issuer;
            _audience = audience;
        }

        public LoginResultDto Login(UserLoginDto dto) => Login(dto, _jwtKey, _issuer, _audience);
    }
}
