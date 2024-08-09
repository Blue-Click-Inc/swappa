using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swappa.Data.Services.Interfaces;
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
        private readonly IServiceManager service;

        public VehicleController(IMediator mediator, IServiceManager service)
        {
            _mediator = mediator;
            this.service = service;
        }

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

        [HttpGet("export-to-excel")]
        [Authorize(Roles = "Admin, Merchant, SuperAdmin")]
        public async Task<IActionResult> DownloadVehicleData()
        {
            var stream = await service.Export.ExportVehicleDataToExcel();
            if(stream == null) return Unauthorized();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Vehicle Report-{DateTime.UtcNow.Ticks}");
        }

        [HttpGet("print-pdf")]
        [Authorize(Roles = "Merchant, Admin, SuperAdmin")]
        public async Task<IActionResult> DownloadReport([FromQuery] DateRangeDto query)
        {
            if(query == null || !query.IsValid)
            {
                return BadRequest("Invalid date range");
            }

            var bytes = await service.Export.VehiclesDetailsReport(query);
            if (bytes == null)
            {
                return BadRequest();
            }

            return File(bytes, "application/pdf", $"Vehicle_Report-{DateTime.UtcNow.Ticks}");
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> FeedbackDashboard() =>
            Ok(await _mediator.Send(new VehicleDashboardQuery()));

        [HttpGet("test-pdf")]
        public IActionResult Test()
        {
            var bytes = service.Export.TestPDF();
            if (bytes == null)
            {
                return BadRequest();
            }

            return File(bytes, "application/pdf", $"Test-{DateTime.UtcNow.Ticks}");
        }

        [HttpGet("sharp-pdf")]
        public IActionResult SharpTest()
        {
            var bytes = service.Export.GeneratePDFSharp();
            if (bytes == null)
            {
                return BadRequest();
            }

            return File(bytes, "application/pdf", $"PDF_Sharp-{Guid.NewGuid()}.pdf");
        }

        [HttpGet("download-template")]
        public async Task<IActionResult> DownloadEmptyCarTemplate()
        {
            var bytes = await service.Export.DownloadCarTemplate();
            if (bytes == null)
            {
                return BadRequest();
            }

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CarTemplate-{Guid.NewGuid()}.xlsx");
        }
    }
}
