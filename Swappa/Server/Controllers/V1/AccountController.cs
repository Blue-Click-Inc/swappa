using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Swappa.Entities.Enums;

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
        public async Task<IActionResult> ConfirmEmail(ConfirmationCommand request)
        {
            request.Origin = HttpContext.Request.Headers["Origin"];
            return Ok(await mediator.Send(request));
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
        {
            request.Origin = HttpContext.Request.Headers["Origin"];
            return Ok(await mediator.Send(request));
        }

        [HttpPut("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordDto request) =>
            Ok(await mediator.Send(new ChangePasswordCommand
            {
                Id = id,
                Request = request
            }));

        [HttpPut("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            request.Origin = HttpContext.Request.Headers["Origin"];
            return Ok(await mediator.Send(new ForgotPasswordCommand
            {
                Request = request
            }));
        }

        [HttpPut("deactivate/{id}")]
        [Authorize]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id) =>
            Ok(await mediator.Send(new DeactivateCommand
            {
                Id= id
            }));

        [HttpPut("toggle-status/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ToggleStatus([FromRoute] Guid id) =>
            Ok(await mediator.Send(new ToggleStatusCommand
            {
                Id = id
            }));

        [HttpPut("{userId}/assign-role/{role}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AddToRole([FromRoute] Guid userId, [FromRoute] SystemRole role) =>
            Ok(await mediator.Send(new AddToRoleCommand
            {
                UserId = userId,
                Role = role
            }));

        [HttpPut("{userId}/remove-role/{role}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> RemoveFromRole([FromRoute] Guid userId, [FromRoute] SystemRole role) =>
            Ok(await mediator.Send(new RemoveFromRoleCommand
            {
                UserId = userId,
                Role = role
            }));
    }
}