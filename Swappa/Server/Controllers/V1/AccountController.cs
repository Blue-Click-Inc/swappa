using Microsoft.AspNetCore.Mvc;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail()
        {
            return Ok();
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return Ok();
        }

        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotPassword()
        {
            return Ok();
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> Deactivate()
        {
            return Ok();
        }

        [HttpPut("reactivate")]
        public async Task<IActionResult> Reactivate()
        {
            return Ok();
        }

        [HttpPut("toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromQuery] string? token)
        {
            return Ok();
        }
    }
}
