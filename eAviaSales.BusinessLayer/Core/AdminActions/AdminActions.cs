using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace eAviaSales.BusinessLayer.Core.AdminActions
{
    public class AdminActions
    {
        private readonly IMapper _mapper;

        public AdminActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<UserDto> GetUsers()
        {
            using var ctx = new UserContext();
            return _mapper.Map<List<UserDto>>(ctx.Users.OrderBy(u => u.Id).ToList());
        }

        protected List<AdminBookingDto> SearchBookingList(AdminBookingsQueryDto query)
        {
            using var ctx = new OrderContext();
            var orders = ctx.Orders.Include(o => o.Items).ToList();

            Dictionary<int, UserData> usersById;
            using (var uCtx = new UserContext())
            {
                usersById = uCtx.Users.ToList().ToDictionary(u => u.Id);
            }

            var bookings = orders.Select(o =>
            {
                usersById.TryGetValue(o.UserId, out var user);
                return new AdminBookingDto
                {
                    OrderId = o.Id,
                    Ref = $"REF{o.Id:D5}",
                    Route = BuildRoute(o),
                    Created = o.CreatedAt.ToString("yyyy-MM-dd"),
                    Status = MapStatus(o.Status),
                    UserFirstName = user?.FirstName ?? "—",
                    UserEmail = user?.Email ?? "—",
                };
            }).ToList();

            if (!string.IsNullOrEmpty(query.Query))
            {
                var q = query.Query.Trim().ToLower();
                bookings = bookings.Where(b =>
                    b.Ref.ToLower().Contains(q) ||
                    b.Route.ToLower().Contains(q) ||
                    b.UserEmail.ToLower().Contains(q) ||
                    b.UserFirstName.ToLower().Contains(q)).ToList();
            }
            if (!string.IsNullOrEmpty(query.DateFrom))
            {
                bookings = bookings.Where(b => string.Compare(b.Created, query.DateFrom) >= 0).ToList();
            }
            if (!string.IsNullOrEmpty(query.DateTo))
            {
                bookings = bookings.Where(b => string.Compare(b.Created, query.DateTo) <= 0).ToList();
            }

            return bookings;
        }

        private string BuildRoute(OrderData order)
        {
            if (order.Items == null || order.Items.Count == 0) return "—";

            using var pCtx = new ProductContext();
            var first = order.Items.First();
            var product = pCtx.Products.FirstOrDefault(p => p.Id == first.ProductId);
            return product?.Route ?? "—";
        }

        private static string MapStatus(OrderStatus s)
        {
            return s switch
            {
                OrderStatus.Paid => "Paid",
                OrderStatus.Pending => "Pending",
                OrderStatus.Cancelled => "Cancelled",
                OrderStatus.Refunded => "Cancelled",
                OrderStatus.Delivered => "Paid",
                _ => "Pending"
            };
        }
    }
}
