using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;

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

        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmationCommand request) =>
            Ok(await mediator.Send(request));

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailDto request) =>
            Ok(await mediator.Send(new ResetPasswordCommand
            {
                Request = request
            }));

        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordDto request) =>
            Ok(await mediator.Send(new ChangePasswordCommand
            {
                Id = id,
                Request = request
            }));

        [HttpPut("forgot-password/{id}")]
        public async Task<IActionResult> ForgotPassword([FromRoute] Guid id, [FromBody] ForgotPasswordDto request) =>
            Ok(await mediator.Send(new ForgotPasswordCommand
            {
                UserId = id,
                Request = request
            }));

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id) =>
            Ok(await mediator.Send(new DeactivateCommand
            {
                Id= id
            }));

        [HttpPut("reactivate/{id}")]
        public async Task<IActionResult> Reactivate([FromRoute] Guid id, TokenDto token) =>
            Ok(await mediator.Send(new ReactivateCommand
            {
                Id = id,
                Token = token
            }));

        [HttpPut("toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] Guid id) =>
            Ok(await mediator.Send(new ToggleStatusCommand
            {
                Id = id
            }));
    }
}