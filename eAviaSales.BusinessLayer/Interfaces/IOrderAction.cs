using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface IOrderAction
    {
        List<OrderDto> GetAll();
        List<OrderDto> GetByUser(ClaimsPrincipal principal);
        OrderDto Create(ClaimsPrincipal principal, OrderDto dto);
        OrderDto? UpdateStatus(int id, ClaimsPrincipal principal, OrderStatus status);
        bool Delete(int id);
    }
}
