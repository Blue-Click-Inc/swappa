using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.User;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id) =>
            Ok(await mediator.Send(new GetUserByIdQuery
            {
                Id = id
            }));

        [HttpPut("details/{id}")]
        public async Task<IActionResult> UpdateDetails([FromRoute] Guid id, [FromBody] UserDetailsForUpdateDto command) =>
            Ok(await mediator.Send(new UpdateUserDetailCommand
            {
                UserId = id,
                Command = command
            }));

        [Authorize(Roles = "User")]
        [HttpPost("feedback/send")]
        public async Task<IActionResult> SendFeedback([FromBody] FeedbackForAddDto request) =>
            Ok(await mediator.Send(new SendUserFeedbackCommand
            {
                Request = request
            }));

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("feedback/all")]
        public async Task<IActionResult> GetUsersFeedbacks([FromQuery] GetUsersFeedbackQuery request) =>
            Ok(await mediator.Send(request));

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("feedback/user")]
        public async Task<IActionResult> GetUserFeedbacks([FromQuery] GetFeedbacksByUserQuery request) =>
            Ok(await mediator.Send(request));

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("feedback/{id}")]
        public async Task<IActionResult> DeleteUserFeedback([FromRoute] Guid id) =>
            Ok(await mediator.Send(new DeleteUserFeedbackCommand
            {
                Id = id
            }));

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPatch("feedback/toggle/{id}")]
        public async Task<IActionResult> ToggleUserFeedback([FromRoute] Guid id) =>
            Ok(await mediator.Send(new ToggleFeedbackDeprecationCommand
            {
                Id = id
            }));

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("feedback/dashboard")]
        public async Task<IActionResult> FeedbackDashboard() =>
            Ok(await mediator.Send(new FeedbackDashboardQuery()));
    }
}
