using AutoMapper;
using eAviaSales.BusinessLayer.Interfaces;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eAviaSales.Api.Controller
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserAction _users;

        public UserController(IMapper mapper, IConfiguration config)
        {
            var bl = new BusinessLayer.BusinessLogic(
                config["Jwt:Key"]!, config["Jwt:Issuer"]!, config["Jwt:Audience"]!, mapper);
            _users = bl.GetUserAction();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll() => Ok(_users.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _users.GetById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserRegisterDto dto)
        {
            var callerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole(nameof(UserRole.Admin));
            if (!isAdmin && callerId != id) return Forbid();

            var result = _users.Update(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            _users.ChangePassword(User, dto);
            return Ok(new { message = "Password changed" });
        }

        [HttpPatch("{id}/role")]
        [Authorize(Roles = "Admin")]
        public IActionResult SetRole(int id, [FromBody] SetUserRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var callerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                var result = _users.SetRole(id, dto.Role, callerId);
                return result == null ? NotFound() : Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/active")]
        [Authorize(Roles = "Admin")]
        public IActionResult SetActive(int id, [FromBody] SetActiveDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var callerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            try
            {
                var result = _users.SetActive(id, dto.IsActive, callerId);
                return result == null ? NotFound() : Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => _users.Delete(id) ? NoContent() : NotFound();
    }
}
