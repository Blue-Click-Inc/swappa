using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Location;
using Swappa.Server.Queries.Location;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator mediatr;

        public LocationController(IMediator mediatr) =>
            this.mediatr = mediatr;

        [HttpGet("countries")]
        public async Task<IActionResult> Get([FromQuery] GetCountriesQuery request) =>
            Ok(await mediatr.Send(request));

        [HttpGet("states/{countryId}")]
        public async Task<IActionResult> Get(string countryId) =>
            Ok(await mediatr.Send(new GetStatesQuery
            {
                CountryId = countryId
            }));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddLocationCommand request) =>
            Ok(await mediatr.Send(request));

        [HttpGet("{entityId}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute]Guid entityId) =>
            Ok(await mediatr.Send(new GetEntityLocationQuery
            {
                EntityId = entityId
            }));

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateLocationCommand request) =>
            Ok(await mediatr.Send(request));

        [HttpDelete("{entityId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid entityId) =>
            Ok(await mediatr.Send(new DeleteLocationCommand
            {
                EntityId = entityId
            }));
    }
}
