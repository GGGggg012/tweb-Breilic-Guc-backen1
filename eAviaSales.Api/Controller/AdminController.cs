using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminAction _admin;
        private readonly IProductAction _products;

        public AdminController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _admin = bl.GetAdminAction();
            _products = bl.GetProductAction();
        }

        [HttpGet("users")]
        public IActionResult GetUsers() => Ok(_admin.GetAllUsers());

        [HttpGet("products")]
        public IActionResult GetProducts() => Ok(_products.GetAllForAdmin());

        [HttpPost("bookings/search")]
        public IActionResult SearchBookings([FromBody] AdminBookingsQueryDto? query)
        {
            return Ok(_admin.SearchBookings(query ?? new AdminBookingsQueryDto()));
        }
    }
}
