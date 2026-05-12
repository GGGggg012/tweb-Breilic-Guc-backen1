using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Core.TicketActions
{
    public class TicketActions
    {
        private readonly IMapper _mapper;

        public TicketActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<TicketDto> SearchTickets(TicketSearchDto query)
        {
            using var ctx = new ProductContext();
            var q = ctx.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(query.From))
                q = q.Where(p => p.Route.Contains(query.From));
            if (!string.IsNullOrEmpty(query.To))
                q = q.Where(p => p.Route.Contains(query.To));
            if (query.MaxPrice.HasValue)
                q = q.Where(p => p.Price <= query.MaxPrice.Value);

            var products = q.ToList();

            if (query.AirlineCodes != null && query.AirlineCodes.Count > 0)
            {
                products = products.Where(p => query.AirlineCodes.Contains(p.AirlineCode)).ToList();
            }
            if (query.Stops != null && query.Stops.Count > 0)
            {
                products = products.Where(p => query.Stops.Contains(p.Stops)).ToList();
            }

            return _mapper.Map<List<TicketDto>>(products);
        }

        protected TicketDto? GetTicketById(string id)
        {
            if (!int.TryParse(id, out var pid)) return null;

            using var ctx = new ProductContext();
            var product = ctx.Products.FirstOrDefault(p => p.Id == pid && p.IsActive);
            return product == null ? null : _mapper.Map<TicketDto>(product);
        }
    }
}
