using AutoMapper;
using eAviaSales.BusinessLayer.Core.TicketActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Functions
{
    public class TicketFlow : TicketActions, ITicketAction
    {
        public TicketFlow(IMapper mapper) : base(mapper) { }

        public List<TicketDto> Search(TicketSearchDto query) => SearchTickets(query);
        public TicketDto? GetById(string id) => GetTicketById(id);
    }
}
