using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAviaSales.Api.Controller
{
    [Route("api/health")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { status = "ok", message = "pong" });

        [HttpGet("version")]
        public IActionResult Version() => Ok(new { version = "v1.0", name = "eAviaSales API" });
    }
}
