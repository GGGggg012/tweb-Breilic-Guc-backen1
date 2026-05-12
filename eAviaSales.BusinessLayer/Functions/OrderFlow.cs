using AutoMapper;
using eAviaSales.BusinessLayer.Core.OrderActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Functions
{
    public class OrderFlow : OrderActions, IOrderAction
    {
        public OrderFlow(IMapper mapper) : base(mapper) { }

        public List<OrderDto> GetAll() => GetAllOrders();
        public List<OrderDto> GetByUser(ClaimsPrincipal principal) => GetOrdersByUser(principal);
        public OrderDto Create(ClaimsPrincipal principal, OrderDto dto) => CreateOrder(principal, dto);
        public OrderDto? UpdateStatus(int id, ClaimsPrincipal principal, OrderStatus status) => UpdateOrder(id, principal, status);
        public bool Delete(int id) => DeleteOrder(id);
    }
}
