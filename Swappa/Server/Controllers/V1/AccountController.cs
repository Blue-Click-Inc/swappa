﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swappa.Server.Commands.Account;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command) =>
            Ok(await mediator.Send(command));


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            request.Origin = HttpContext.Request.Headers["Origin"];
            return Ok(await mediator.Send(request));
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
