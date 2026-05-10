using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IUserRegAction
    {
        void Register(RegisterAccountDto dto, string? configuredBootstrapSecret);
    }
}
