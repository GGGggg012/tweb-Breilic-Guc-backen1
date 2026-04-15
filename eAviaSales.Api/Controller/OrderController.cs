using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAction _orders;

        public OrderController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _orders = bl.GetOrderAction();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetAll() => Ok(_orders.GetAll());

        [HttpGet("my")]
        public IActionResult GetMyOrders() => Ok(_orders.GetByUser(User));

        [HttpPost]
        public IActionResult Create([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return StatusCode(201, _orders.Create(User, dto));
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var result = _orders.UpdateStatus(id, User, dto.Status);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => _orders.Delete(id) ? NoContent() : NotFound();
    }
}
