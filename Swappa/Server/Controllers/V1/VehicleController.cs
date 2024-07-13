using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Vehicle;
using Swappa.Server.Queries.Vehicle;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VehicleController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] VehicleToCreateDto command) =>
            Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] VehicleForUpdateDto command) =>
            Ok(await _mediator.Send(new UpdateVehicleCommand
            {
                Id = id,
                Request = command
            }));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id) =>
            Ok(await _mediator.Send(new GetVehicleByIdQuery
            {
                Id = id,
            }));

        [HttpGet("{merchantId}/merchant")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid merchantId, [FromQuery] VehicleQueryDto query) =>
            Ok(await _mediator.Send(new GetVehicleByMerchantQuery
            {
                MerchantId = merchantId,
                Query = query
            }));

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetAllVehiclesQuery query) =>
            Ok(await _mediator.Send(query));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) =>
            Ok(await _mediator.Send(new DeleteVehicleCommand
            {
                Id = id,
            }));
    }
}
