using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.ContactMessage;
using Swappa.Server.Queries.ContactMessages;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/contact-message")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactMessageController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateContactMessageCommand request) =>
            Ok(await mediator.Send(request));

        [HttpPut("toggle-read/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ToggleRead([FromRoute] Guid id) =>
            Ok(await mediator.Send(new ToggleContactMessageIsReadCommand
            {
                Id = id
            }));

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetPaginatedList([FromQuery] PageDto request) =>
            Ok(await mediator.Send(new GetContactMessageQuery
            {
                Query = request
            }));

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) =>
            Ok(await mediator.Send(new GetContactMessageByIdQuery
            {
                Id = id
            }));
    }
}
