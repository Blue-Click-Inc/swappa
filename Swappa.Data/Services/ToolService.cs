using Hangfire.Console;
using Hangfire.Server;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.Extensions;
using System.Drawing;

namespace Swappa.Data.Services
{
    public class ToolService : IToolService
    {
        private readonly IRepositoryManager repository;

        public ToolService(IRepositoryManager repository)
        {
            this.repository = repository;
        }

        public async Task<Stream> ExportVehicleDataToExcel(Guid entityId)
        {
            //Getting data
            var vehicles = await Task.Run(() => 
                repository.Vehicle
                    .FindAsQueryable(v => !v.IsDeprecated && 
                    v.CreatedAt >= DateTime.UtcNow.AddYears(-1))
                    .ToList());
            //Initializing stream
            var stream = new MemoryStream();
            using var xlPackage = new ExcelPackage(stream);
            var workSheet = xlPackage.Workbook.Worksheets.Add("Vehicles Data");

            //Styling
            var customStyle = xlPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");
            customStyle.Style.Font.Color.SetColor(Color.Red);

            //First row
            var startRow = 3;
            var row = startRow;
            var totalPrice = 0M;

            workSheet.Cells["A1"].Value = "Vehicle Data Report";
            workSheet.Row(1).Height = 40;
            using var titleHeaderRow = workSheet.Cells["A1:M1"];
            titleHeaderRow.SetRowStyles(20, Color.White, Color.Black, 
                ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center, true, true);

            //Set title headers of the Report
            workSheet.SetHeaderValues(GetTitleHeader(), 2, true);

            using var headerRow = workSheet.Cells["A2:M2"];
            workSheet.Row(2).Height = 25;
            headerRow.SetRowStyles(null, Color.Black, Color.DarkGray, 
                ExcelVerticalAlignment.Center, ExcelHorizontalAlignment.Center, false, true);

            foreach (var vehicle in vehicles)
            {
                //Make cell 1
                workSheet.SetCellValue(row, 1, vehicle.Make, true, 15, 40);
                //Model cell 2
                workSheet.SetCellValue(row, 2, vehicle.Model, true, 15, 40);
                //Year cell 3
                workSheet.SetCellValue(row, 3, vehicle.Year, true);
                //Color 4
                workSheet.SetCellValue(row, 4, vehicle.Color, true);
                //Trim cell 5
                workSheet.SetCellValue(row, 5, vehicle.Trim, true, 10, 30);
                //Transmission cell 6
                workSheet.SetCellValue(row, 6, vehicle.Transmission.GetDescription(), true, 15, 40);
                //Engine 7
                workSheet.SetCellValue(row, 7, vehicle.Engine.GetDescription(), true);
                //DriveTrain cell 8
                workSheet.SetCellValue(row, 8, vehicle.DriveTrain.GetDescription(), true, 10, 30);
                //VIN cell 9
                workSheet.SetCellValue(row, 9, vehicle.VIN, true);
                //Interior cell 10
                workSheet.SetCellValue(row, 10, vehicle.Interior, true, 10, 30);
                //Odometer cell 11
                workSheet.SetCellValue(row, 11, vehicle.Odometer, true, 10, 20, "#,##");
                //Date Created cell 12
                workSheet.SetCellValue(row, 12, $"{vehicle.CreatedAt:M}, {vehicle.CreatedAt.Year}", true, 15, 40);
                //Price Cell 13
                workSheet.SetCellValue(row, 13, vehicle.Price, true, 15, 40, "#,##0.00");

                //Increment total price
                totalPrice += (decimal)vehicle.Price;
                row++;
            }

            var cells = $"A{row}:M{row}";
            var totalPriceRow = workSheet.Cells[cells];
            totalPriceRow.SetRowStyles(null, null, Color.DarkGray, null, null, bold: true);

            workSheet.SetCellValue(row, 1, $"Total Price for {vehicles.Count} Vehicles", true);
            workSheet.SetCellValue(row, 13, totalPrice, true, 15, 40, "#,##0.00");

            xlPackage.Workbook.Properties.Title = $"Vehicle Data Report-{DateTime.UtcNow.Ticks}";
            xlPackage.Workbook.Properties.Author = "Ojo Toba Rufus";
            xlPackage.Save();

            stream.Position = 0;
            return stream;
        }

        public async Task VehicleBulkUpload(List<Vehicle> vehicles, List<Entities.Models.Image> images, Guid userId, PerformContext context)
        {
            var locations = new List<EntityLocation>();

            if (userId.IsNotEmpty())
            {
                context.WriteLine($"Getting location for user: {userId}");
                var userLocation = await repository.Location.FindOneAsync(_ => _.EntityId.Equals(userId));
                if(userLocation != null)
                {
                    context.WriteLine($"Initializing locations for {vehicles.Count} vehicle records");
                    vehicles.ForEach(v =>
                    {
                        locations.Add(new EntityLocation
                        {
                            EntityId = v.Id,
                            EntityType = EntityType.Vehicle,
                            City = userLocation.City,
                            Country = userLocation.Country,
                            State = userLocation.State,
                            PostalCode = userLocation.PostalCode,
                            CountryId = userLocation.CountryId,
                            StateId = userLocation.StateId,
                            Coordinate = userLocation.Coordinate
                        });
                    });
                }
            }

            context.WriteLine($"Adding {vehicles.Count} vehicle records");
            await repository.Vehicle.AddAsync(vehicles);
            context.WriteLine($"Successfully added {vehicles.Count} vehicle records. Now adding the respective images.");

            await repository.Image.AddAsync(images);
            context.WriteLine($"Successfully added {images.Count} image records. Now adding vehicle locations.");

            await repository.Location.AddAsync(locations);
            context.WriteLine($"Successfully added {locations.Count} location records.");
        }

        private static Dictionary<string, string> GetTitleHeader()
        {
            return new Dictionary<string, string>
            {
                { "A2", "Make" },
                { "B2", "Model" },
                { "C2", "Year of Mfg." },
                { "D2", "Color" },
                { "E2", "Trim" },
                { "F2", "Transmission" },
                { "G2", "Engine" },
                { "H2", "Drive Train" },
                { "I2", "VIN" },
                { "J2", "Interior" },
                { "K2", "Odometer" },
                { "L2", "Date Added" },
                { "M2", "Price (NGN)" }
            };
        }
    }
}
