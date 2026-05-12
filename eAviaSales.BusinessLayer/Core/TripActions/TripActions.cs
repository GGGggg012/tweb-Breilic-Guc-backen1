using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Core.TripActions
{
    public class TripActions
    {
        private readonly IMapper _mapper;

        public TripActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<TripDto> GetUserTrips(ClaimsPrincipal principal, TripsQueryDto query)
        {
            var userId = ExtractUserId(principal);

            using var ctx = new OrderContext();
            var orders = ctx.Orders.Include(o => o.Items)
                .Where(o => o.UserId == userId).ToList();

            var trips = orders.Select(BuildTrip).ToList();

            if (!string.IsNullOrEmpty(query.Status) && query.Status != "all")
            {
                trips = trips.Where(t => t.Status == query.Status).ToList();
            }
            if (!string.IsNullOrEmpty(query.Query))
            {
                var q = query.Query.Trim().ToLower();
                trips = trips.Where(t =>
                    t.RouteLabel.ToLower().Contains(q) ||
                    t.Pnr.ToLower().Contains(q) ||
                    t.CityHint.ToLower().Contains(q)).ToList();
            }

            return trips;
        }

        protected TripDto? GetTripById(ClaimsPrincipal principal, string id)
        {
            var userId = ExtractUserId(principal);
            if (!int.TryParse(id.Replace("trip-", ""), out var orderId))
                return null;

            using var ctx = new OrderContext();
            var order = ctx.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == orderId && o.UserId == userId);
            return order == null ? null : BuildTrip(order);
        }

        private TripDto BuildTrip(OrderData order)
        {
            string routeLabel = "—";
            string airline = "";
            string dateRange = "";
            string cityHint = "";

            if (order.Items != null && order.Items.Count > 0)
            {
                using var pCtx = new ProductContext();
                var firstItem = order.Items.First();
                var product = pCtx.Products.FirstOrDefault(p => p.Id == firstItem.ProductId);
                if (product != null)
                {
                    routeLabel = product.Route;
                    airline = product.Airline;
                    dateRange = product.FlightDate;
                    var arrow = product.Route.Split('→');
                    cityHint = arrow.Length > 1 ? arrow[1].Trim() : product.Route;
                }
            }

            string status = order.Status switch
            {
                OrderStatus.Cancelled => "cancelled",
                OrderStatus.Refunded => "cancelled",
                OrderStatus.Delivered => "past",
                _ => "upcoming"
            };

            return new TripDto
            {
                Id = $"trip-{order.Id:D3}",
                RouteLabel = routeLabel,
                Pnr = $"TRIP-{order.Id:D3}",
                Status = status,
                CityHint = cityHint,
                DateRange = dateRange,
                Airline = airline,
                CheckInAvailable = status == "upcoming"
            };
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
