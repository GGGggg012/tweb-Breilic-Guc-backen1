using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/tickets")]
    [ApiController]
    [AllowAnonymous]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketAction _tickets;

        public TicketsController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _tickets = bl.GetTicketAction();
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody] TicketSearchDto dto)
        {
            return Ok(_tickets.Search(dto ?? new TicketSearchDto()));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var ticket = _tickets.GetById(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }
    }
}
