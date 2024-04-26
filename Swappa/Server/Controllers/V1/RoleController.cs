using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Role;
using Swappa.Server.Queries.Role;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/role")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator mediator;

        public RoleController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRoleCommand command) =>
            Ok(await mediator.Send(command));


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllRolesQuery request) =>
            Ok(await mediator.Send(request));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id) =>
            Ok(await mediator.Send(new GetRolebyIdQuery
            {
                Id = id
            }));

        //[HttpPut]
        //public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleCommand command)
        //{
        //    return Ok();
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromRoute] Guid id)
        //{
        //    return Ok();
        //}
    }
}
