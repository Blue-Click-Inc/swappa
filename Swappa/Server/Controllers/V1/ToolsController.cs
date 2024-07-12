using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Tools;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/tools")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IMediator mediator;
        public ToolsController(IMediator mediator)=>
            this.mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> UploadBulkVehicle(IFormFile file) =>
            Ok(await mediator.Send(new UploadBulkVehicleCommand
            {
                File = file
            }));
    }
}
