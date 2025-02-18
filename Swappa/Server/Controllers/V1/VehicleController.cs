﻿using MediatR;
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

        [HttpPost, Authorize(Roles = "SuperAdmin, Admin, Merchant")]
        public async Task<IActionResult> PostAsync([FromForm] VehicleToCreateDto command) =>
            Ok(await _mediator.Send(new AddVehicleCommand
            {
                Request = command,
            }));

        [HttpPut("{id}"), Authorize(Roles = "SuperAdmin, Admin, Merchant")]
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

        [HttpGet("{merchantId}/merchant"), Authorize(Roles = "SuperAdmin, Admin, Merchant")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid merchantId, [FromQuery] VehicleQueryDto query) =>
            Ok(await _mediator.Send(new GetVehicleByMerchantQuery
            {
                MerchantId = merchantId,
                Query = query
            }));

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetAllVehiclesQuery query) =>
            Ok(await _mediator.Send(query));

        [HttpDelete("{id}"), Authorize(Roles = "SuperAdmin, Admin, Merchant")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) =>
            Ok(await _mediator.Send(new DeleteVehicleCommand
            {
                Id = id,
            }));

        [HttpPost("toggle-favorite")]
        [Authorize]
        public async Task<IActionResult> ToggleFavorite([FromBody] IdDto request) =>
            Ok(await _mediator.Send(new ToggleFavoriteVehicleCommand
            {
                VehicleId = request.Id
            }));

        [HttpGet("{userId}/favorites")]
        public async Task<IActionResult> GeFavorites([FromRoute] Guid userId, [FromQuery] GetAllVehiclesQuery query) =>
            Ok(await _mediator.Send(new GetFavoriteVehiclesQuery
            {
                UserId = userId,
                Request = new PageDto
                {
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    SearchTerm = query.SearchTerm
            }}));

        [Authorize]
        [HttpGet("favorite-count/{userId}")]
        public async Task<IActionResult> FavoriteCount([FromRoute] Guid userId) =>
            Ok(await _mediator.Send(new GetFavoriteVehicleCountQuery
            {
                UserId = userId
            }));

        [Authorize]
        [HttpGet("is-favorite/{id}")]
        public async Task<IActionResult> IsFavorite([FromRoute] Guid id) =>
            Ok(await _mediator.Send(new IsFavoriteVehicleQuery
            {
                Id = id
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

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest()
        {
            return Ok(await service.Export.TestDict());
        }
    }
}
