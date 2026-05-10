using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/session")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserLoginAction _login;

        public AuthController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _login = bl.GetUserLoginAction();
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = _login.Login(dto);
            return Ok(new
            {
                token = result.Token,
                role = result.Role.ToLower(),
                userId = result.UserId,
                username = result.Username
            });
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public IActionResult Status() => Ok(new { status = "active" });
    }
}
