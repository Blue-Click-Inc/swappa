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
    }
}
