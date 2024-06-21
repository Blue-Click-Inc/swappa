using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    }
}
