using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Core.OrderActions
{
    public class OrderActions
    {
        private readonly IMapper _mapper;

        public OrderActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<OrderDto> GetAllOrders()
        {
            using var ctx = new OrderContext();
            var orders = ctx.Orders.Include(o => o.Items).ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        protected List<OrderDto> GetOrdersByUser(ClaimsPrincipal principal)
        {
            var userId = ExtractUserId(principal);
            using var ctx = new OrderContext();
            var orders = ctx.Orders.Include(o => o.Items)
                .Where(o => o.UserId == userId).ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        protected OrderDto CreateOrder(ClaimsPrincipal principal, OrderDto dto)
        {
            var userId = ExtractUserId(principal);

            if (dto.Items == null || dto.Items.Count == 0)
                throw new InvalidOperationException("Order must contain at least one item");

            decimal total = 0;
            var orderItems = new List<OrderItemData>();

            using (var productCtx = new ProductContext())
            {
                foreach (var itemDto in dto.Items)
                {
                    var product = productCtx.Products.FirstOrDefault(p => p.Id == itemDto.ProductId)
                        ?? throw new InvalidOperationException($"Product {itemDto.ProductId} not found");
                    if (!product.IsActive)
                        throw new InvalidOperationException($"Product {itemDto.ProductId} is not available");

                    product.DecreaseStock(itemDto.Qua);

                    var price = product.Price;
                    var info = string.IsNullOrEmpty(itemDto.ProductInfo)
                        ? $"{product.Airline} - {product.Route}"
                        : itemDto.ProductInfo;

                    orderItems.Add(new OrderItemData
                    {
                        ProductId = product.Id,
                        ProductInfo = info,
                        Qua = itemDto.Qua,
                        Price = price
                    });

                    total += price * itemDto.Qua;
                }

                productCtx.SaveChanges();
            }

            var order = new OrderData
            {
                UserId = userId,
                Items = orderItems,
                TotalPrice = total,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            using (var orderCtx = new OrderContext())
            {
                orderCtx.Orders.Add(order);
                orderCtx.SaveChanges();
            }

            return _mapper.Map<OrderDto>(order);
        }

        protected OrderDto? UpdateOrder(int id, ClaimsPrincipal principal, OrderStatus status)
        {
            using var ctx = new OrderContext();
            var order = ctx.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (order == null) return null;

            order.Status = status;
            ctx.SaveChanges();
            return _mapper.Map<OrderDto>(order);
        }

        protected bool DeleteOrder(int id)
        {
            using var ctx = new OrderContext();
            var order = ctx.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return false;
            ctx.Orders.Remove(order);
            ctx.SaveChanges();
            return true;
        }

        protected static int ExtractUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(value))
                throw new UnauthorizedAccessException("User identity not found in token");
            return int.Parse(value);
        }
    }
}
