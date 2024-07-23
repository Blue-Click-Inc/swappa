using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using System.Text;

namespace Swappa.Shared.Extensions
{
    public static class Statics
    {
        public static string GetAccountConfirmationTemplate(string url, string name)
        {
            string body = string.Empty;

            var folderName = Path.Combine("wwwroot", "Templates", "AccountConfirmation.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return body;

            var msgBody = body.Replace("{email_link}", url).
                Replace("{name}", name).
                Replace("{year}", $"{DateTime.Now.Year}");

            return msgBody;
        }

        public static string GetPasswordResetTemplate(string emailLink)
        {
            string body = string.Empty;
            var folderName = Path.Combine("wwwroot", "Templates", "PasswordReset.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return body;

            var msgBody = body.Replace("{email_link}", emailLink).
                Replace("{year}", DateTime.Now.Year.ToString());

            return msgBody;
        }

        public static string GetInvoicePdf(TestDetailsClass details)
        {
            string body = string.Empty;
            var folderName = Path.Combine("wwwroot", "PDF", "InvoicePdf.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return body;

            body = body
                .Replace("{amount}", $"{details.Amount:#,##0.00}")
                .Replace("{date_initiated}", $"{details.DateInitiated:M}, {details.DateInitiated.Year}")
                .Replace("{time_initiated}", $"{details.DateInitiated:t}")
                .Replace("{reference_number}", details.Reference)
                .Replace("{sender_name}", details.SenderName)
                .Replace("{bank_name}", details.BankName)
                .Replace("{naration}", details.Naration)
                .Replace("{customer_name}", details.CustomerName)
                .Replace("{trans_type}", details.Type);

            return body;
        }

        public static string GetVehicleReportPdf(VehiclesReportDto details)
        {
            string body = string.Empty;
            var folderName = Path.Combine("wwwroot", "PDF", "VehicleReportPdf.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return body;

            var sb = new StringBuilder();
            var contentSb = new StringBuilder();
            body = body
                .Replace("{{total_amount}}", $"NGN{details.TotalPrice:#,##0.00}")
                .Replace("{{date_range}}", $"{details.FromDate:M}, {details.FromDate.Year} - {details.ToDate:M}, {details.ToDate.Year}")
                .Replace("{{merchant_name}}", $"{details.MerchantName}")
                .Replace("{{number_of_vehicles}}", $"{details.NumOfVehicles:#,##}")
                .Replace("{{average_price}}", $"NGN{details.AveragePrice:#,##0.00}")
                .Replace("{{lowest_price}}", $"NGN{details.LowestPriced:#,##0.00}")
                .Replace("{{highest_price}}", $"NGN{details.HighestPrice:#,##0.00}");

            contentSb.Append(GetTransmissionSection(details.Transmission))
                .AppendLine()
                .Append(GetEngineSection(details.Engine))
                .AppendLine()
                .Append(GetDriveTrainSection(details.DriveTrain));

            sb.Append(body)
                .AppendLine()
                .Replace("{{additional_information}}", contentSb.ToString());

            return sb.ToString();
        }

        public static string GetEngineSection(Dictionary<Engine, int> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty() && keyValuePairs.Any(x => x.Value > 0))
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTable.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{headings}}", "Details By Engine Types")
                    .Replace("{{table_contents}}", GetEngineContents(keyValuePairs));
            }
            
            return tableBody;
        }

        public static string GetEngineContents(Dictionary<Engine, int> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if(pair.Value > 0)
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTableData.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{title_text}}", pair.Key.GetDescription())
                        .Replace("{{total_numbers}}", $"{pair.Value:#,##}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }

        public static string GetDriveTrainSection(Dictionary<DriveTrain, int> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty() && keyValuePairs.Any(x => x.Value > 0))
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTable.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{headings}}", "Details By Drive Trains")
                    .Replace("{{table_contents}}", GetDriveTrainContents(keyValuePairs));
            }

            return tableBody;
        }

        public static string GetDriveTrainContents(Dictionary<DriveTrain, int> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if (pair.Value > 0)
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTableData.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{title_text}}", pair.Key.GetDescription())
                        .Replace("{{total_numbers}}", $"{pair.Value:#,##}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }

        public static string GetTransmissionSection(Dictionary<Transmission, int> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty() && keyValuePairs.Any(x => x.Value > 0))
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTable.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{headings}}", "Details By Transmission Types")
                    .Replace("{{table_contents}}", GetTransmissionContents(keyValuePairs));
            }

            return tableBody;
        }

        public static string GetTransmissionContents(Dictionary<Transmission, int> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if (pair.Value > 0)
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "VehicleTableData.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{title_text}}", pair.Key.GetDescription())
                        .Replace("{{total_numbers}}", $"{pair.Value:#,##}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
