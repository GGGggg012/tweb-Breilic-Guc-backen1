using eAviaSales.BusinessLayer.Core.UserActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Functions
{
    public class UserRegFlow : UserAuthActions, IUserRegAction
    {
        public new void Register(RegisterAccountDto dto, string? configuredBootstrapSecret) =>
            base.Register(dto, configuredBootstrapSecret);
    }
}