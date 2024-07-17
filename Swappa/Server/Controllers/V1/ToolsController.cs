using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Swappa.Data.Services.Interfaces;
using Swappa.Server.Commands.Tools;

namespace Swappa.Server.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/tools")]
    [ApiController]
    //[Authorize(Roles = "Admin, SuperAdmin")]
    public class ToolsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IToolService toolService;

        public ToolsController(IMediator mediator, IToolService toolService)
        {
            this.mediator = mediator;
            this.toolService = toolService;
        }

        [HttpPost("vehicle-upload-bulk")]
        public async Task<IActionResult> UploadBulkVehicle(List<IFormFile> files) =>
            Ok(await mediator.Send(new UploadBulkVehicleCommand
            {
                File = files.FirstOrDefault()
            }));

        [HttpPost("export-vehicle-data/{entityId}")]
        public async Task<IActionResult> DownloadVehicleData(Guid entityId)
        {
            var stream = await toolService.ExportVehicleDataToExcel(entityId);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Vehicle Report-{DateTime.UtcNow.Ticks}");
        }
    }
}
