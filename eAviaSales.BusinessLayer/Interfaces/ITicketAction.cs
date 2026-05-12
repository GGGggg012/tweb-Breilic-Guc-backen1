using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface ITicketAction
    {
        List<TicketDto> Search(TicketSearchDto query);
        TicketDto? GetById(string id);
    }
}
