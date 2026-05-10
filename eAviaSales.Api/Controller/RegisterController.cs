using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/reg")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRegAction _reg;
        private readonly string? _bootstrapSecret;

        public RegisterController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _reg = bl.GetUserRegAction();
            _bootstrapSecret = config["Security:AdminBootstrapSecret"];
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterAccountDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _reg.Register(dto, _bootstrapSecret);
            return StatusCode(201, new { message = "Registered successfully" });
        }
    }
}
