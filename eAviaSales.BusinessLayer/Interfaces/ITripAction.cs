using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Interfaces
{
    public interface ITripAction
    {
        List<TripDto> GetTrips(ClaimsPrincipal principal, TripsQueryDto query);
        TripDto? GetById(ClaimsPrincipal principal, string id);
    }
}
