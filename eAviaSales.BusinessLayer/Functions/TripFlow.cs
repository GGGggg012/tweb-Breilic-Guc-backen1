using AutoMapper;
using eAviaSales.BusinessLayer.Core.TripActions;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using System.Security.Claims;

namespace eAviaSales.BusinessLayer.Functions
{
    public class TripFlow : TripActions, ITripAction
    {
        public TripFlow(IMapper mapper) : base(mapper) { }

        public List<TripDto> GetTrips(ClaimsPrincipal principal, TripsQueryDto query) => GetUserTrips(principal, query);
        public TripDto? GetById(ClaimsPrincipal principal, string id) => GetTripById(principal, id);
    }
}
