using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Queries.Charts;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/charts")]
    [ApiController]
    //[Authorize]
    public class ChartsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ChartsController(IMediator mediator) =>
            this.mediator = mediator;

        [HttpGet("top-five")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetTopFiveVehiclesChart() =>
            Ok(await mediator.Send(new GetTopFiveVehiclesChartQuery()));

        [HttpGet("price-range-and-engine")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetPriceRangeAndEngineCharts() =>
            Ok(await mediator.Send(new PriceRangesAndEngineLineChartsQuery()));
    }
}
