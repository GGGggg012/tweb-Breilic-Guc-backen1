using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/trips")]
    [ApiController]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly ITripAction _trips;

        public TripsController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _trips = bl.GetTripAction();
        }

        [HttpPost]
        public IActionResult GetTrips([FromBody] TripsQueryDto? query)
        {
            return Ok(_trips.GetTrips(User, query ?? new TripsQueryDto()));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var trip = _trips.GetById(User, id);
            return trip == null ? NotFound() : Ok(trip);
        }
    }
}
