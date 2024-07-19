using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Server.Commands.Tools;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/tools")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ToolsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ToolsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("vehicle-upload-bulk")]
        public async Task<IActionResult> UploadBulkVehicle(List<IFormFile> files) =>
            Ok(await mediator.Send(new UploadBulkVehicleCommand
            {
                File = files.FirstOrDefault()
            }));
    }
}
