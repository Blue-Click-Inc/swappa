using Hangfire;
using MediatR;
using OfficeOpenXml;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Tools;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Tools
{
    public class UploadBulkVehicleCommandHandler : IRequestHandler<UploadBulkVehicleCommand, ResponseModel<string>>
    {
        private const int MAX_SIZE = 5242880;
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public UploadBulkVehicleCommandHandler(ApiResponseDto response, IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(UploadBulkVehicleCommand request, CancellationToken cancellationToken)
        {
            if(request.IsNotNull() && request.File.IsNotNull() && request.File.Length > 0)
            {
                using var stream = new MemoryStream(MAX_SIZE);
                await request.File.CopyToAsync(stream, cancellationToken);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                var rowCount = worksheet.Dimension.Rows;
                if (worksheet != null)
                {
                    var vehicles = new List<Vehicle>();
                    var images = new List<Image>();
                    for (int row = 2; row <= rowCount; row++) // Omit the header
                    {
                        var imageUrl = worksheet.Cells[row, 14].Value.ToString();

                        var vehicle = new Vehicle
                        {
                            Make = worksheet.Cells[row, 2]?.Value?.ToString() ?? string.Empty,
                            Model = worksheet.Cells[row, 3]?.Value?.ToString() ?? string.Empty,
                            Year = int.TryParse(worksheet.Cells[row, 4]?.Value?.ToString(), out int year) ? year : DateTime.Now.Year,
                            Trim = worksheet.Cells[row, 5]?.Value?.ToString() ?? string.Empty,
                            Engine = GetEngine(worksheet.Cells[row, 6]?.Value?.ToString()),
                            Odometer = int.TryParse(worksheet.Cells[row, 7]?.Value?.ToString(), out int odometer) ? odometer : default,
                            Color = worksheet.Cells[row, 8]?.Value?.ToString() ?? string.Empty,
                            Interior = worksheet.Cells[row, 9]?.Value?.ToString() ?? string.Empty,
                            Transmission = GetTransmission(worksheet.Cells[row, 10]?.Value?.ToString()),
                            DriveTrain = GetDriveTrain(worksheet.Cells[row, 11]?.Value?.ToString()),
                            Price = double.TryParse(worksheet.Cells[row, 12]?.Value?.ToString(), out double price) ? price : default,
                            VIN = worksheet.Cells[row, 13]?.Value?.ToString() ?? string.Empty
                        };

                        if (!string.IsNullOrWhiteSpace(imageUrl))
                        {
                            images.Add(new Image
                            {
                                Url = imageUrl,
                                IsMain = true,
                                VehicleId = vehicle.Id,
                            });
                        }
                        vehicles.Add(vehicle);
                    }

                    Guid.TryParse(repository.Common.GetLoggedInUserId(), out var userId);
                    BackgroundJob.Enqueue<IToolService>(_ => _.VehicleBulkUpload(vehicles, images, userId, null!));
                    return response.Process<string>(new ApiOkResponse<string>($"Job to add {vehicles.Count} vehicle records started!"));
                }

                return response.Process<string>(new BadRequestResponse("Could not read the worksheet."));
            }

            return response.Process<string>(new BadRequestResponse("Invalid file uploaded. Please choose a valid file to continue."));
        }

        private static Transmission GetTransmission(string? transmission)
        {
            return transmission switch
            {
                "Manual" => Transmission.Manual,
                "Automatic" => Transmission.Automatic,
                _ => Transmission.None,
            };
        }

        private static Engine GetEngine(string? engine)
        {
            return engine switch
            {
                "4 Cyl." => Engine.FourCylinders,
                "6 Cyl." => Engine.SixCylinders,
                "8 Cyl." => Engine.EightCylinders,
                "12 Cyl." => Engine.TwelveCylinders,
                "16 Cyl." => Engine.SixteenCylinders,
                "20 Cyl." => Engine.TwentyCylinders,
                "24 Cyl." => Engine.TwentyFourCylinders,
                _ => Engine.None,
            };
        }
        
        private static DriveTrain GetDriveTrain(string? driveTrain)
        {
            return driveTrain switch
            {
                "RWD" => DriveTrain.RWD,
                "FWD" => DriveTrain.FWD,
                "AWD" => DriveTrain.AWD,
                "2WD" => DriveTrain.TwoWD,
                "4WD" => DriveTrain.FourWD,
                _ => DriveTrain.None,
            };
        }
    }
}
