using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Queries.Faq;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/faq")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly IMediator mediator;

        public FaqController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFaqCommand command) =>
            Ok(await mediator.Send(command));

        [HttpPost("multiple")]
        public async Task<IActionResult> AddMany([FromBody] List<FaqToCreateDto> request) =>
            Ok(await mediator.Send(new CreateFaqsCommand
            {
                Requests = request
            }));


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllFaqQuery request) =>
            Ok(await mediator.Send(request));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id) =>
            Ok(await mediator.Send(new GetFaqByIdQuery
            {
                Id = id
            }));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) =>
            Ok(await mediator.Send(new DeleteFaqCommand
            {
                Id = id
            }));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FaqToUpdateDto command) =>
            Ok(await mediator.Send(new UpdateFaqCommand
            {
                Id = id,
                Request = command
            }));
    }
}
