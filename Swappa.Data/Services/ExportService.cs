using DinkToPdf;
using DinkToPdf.Contracts;
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
    public class ExportService : IExportService
    {
        private readonly IRepositoryManager repository;
        private readonly IConverter converter;

        public ExportService(IRepositoryManager repository,
            IConverter converter)
        {
            this.repository = repository;
            this.converter = converter;
        }

        public async Task<Stream> ExportVehicleDataToExcel()
        {
            var userRoles = await repository.Common.GetUserRoles();
            if (userRoles.IsNotNullOrEmpty() && userRoles.IsInOneOrMoreRoles(SystemRole.SuperAdmin, SystemRole.Admin, SystemRole.Merchant))
            {
                var userId = repository.Common.GetLoggedInUserId();
                var vehicles = new List<Vehicle>();
                //Getting data
                var vehicleQuery = repository.Vehicle
                        .FindAsQueryable(v => !v.IsDeprecated && v.CreatedAt >= DateTime.UtcNow.AddYears(-1));

                if (userRoles.Contains(SystemRole.Merchant))
                {
                    vehicleQuery = vehicleQuery.Where(v => !v.IsDeprecated);
                }

                vehicles = await Task.Run(() => vehicleQuery.ToList());
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

            return null!;
        }

        public byte[] ExportToPdf()
        {
            var html = Statics.GetInvoicePdf();
            if (!html.IsNotNullOrEmpty())
            {
                return null!;
            }

            var globalSettings = GetPdfSettings();
            var objectSettings = GetPdfObjectSettings(html);
            var document = GetPdfDocument(globalSettings, objectSettings);

            return converter.Convert(document);
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

        private GlobalSettings GetPdfSettings()
        {
            return new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                ImageDPI = 500,
                ImageQuality = 500,
                Margins = new MarginSettings
                {
                    Top = 10,
                    Bottom = 10,
                    Left = 10,
                    Right = 10
                },
                DocumentTitle = "Invoice"
            };
        }

        private ObjectSettings GetPdfObjectSettings(string html)
        {
            return new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                WebSettings = { DefaultEncoding = "utf-8" },
                //HeaderSettings = { FontSize = 12, Right = "Page {page} of {toPage}", Line = true, Spacing = 2.812 },
                FooterSettings = { FontSize = 12, Right = $"© {DateTime.Now.Year}" }
            };
        }

        private HtmlToPdfDocument GetPdfDocument(GlobalSettings globalSettings, ObjectSettings objectSettings)
        {
            return new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }
    }
}
