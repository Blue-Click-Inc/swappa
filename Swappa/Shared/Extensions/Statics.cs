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

        public static string TestPDF(VehiclesReportDto details)
        {
            string body = string.Empty;
            var folderName = Path.Combine("wwwroot", "PDF", "VehicleDataPDF.html");
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (File.Exists(filepath))
                body = File.ReadAllText(filepath);
            else
                return body;

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
                .Replace("{{total_price_of_items}}", $"{details.TotalPrice:#,##0.00}")
                .Replace("{{date_range_string}}", $"{details.FromDate.ToDateStringFormat()} - {details.ToDate.ToDateStringFormat()}")
                .Replace("{{merchant_name}}", $"{details.MerchantName}")
                .Replace("{{total_number_of_items}}", $"{details.NumOfVehicles:#,##}")
                .Replace("{{address_city_state}}", $"{details.Contact.City}, {details.Contact.State}")
                .Replace("{{country_dot_postal_code}}", $"{details.Contact.Country}. {details.Contact.PostalCode}")
                .Replace("{{email_address}}", details.Contact.Email)
                .Replace("{{phone_number}}", details.Contact.PhoneNumber);

            contentSb.Append(GetTransmissionSection(details.Transmission))
                .AppendLine()
                .Append(GetEngineSection(details.Engine))
                .AppendLine()
                .Append(GetDriveTrainSection(details.DriveTrain));

            sb.Append(body)
                .AppendLine()
                .Replace("{{categorized_details}}", contentSb.ToString());

            return sb.ToString();
        }

        public static string GetEngineSection(Dictionary<Engine, List<Vehicle>> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty())
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportDetails.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{category_header_title}}", "Details By Engine Types")
                    .Replace("{{category_item_list}}", GetEngineContents(keyValuePairs));
            }
            
            return tableBody;
        }

        public static string GetEngineContents(Dictionary<Engine, List<Vehicle>> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if(pair.Value.IsNotNullOrEmpty())
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportItem.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{category_item_title}}", pair.Key.GetDescription())
                        .Replace("{{category_item_count}}", $"{pair.Value.Count:#,##}")
                        .Replace("{{category_item_total_price}}", $"{pair.Value.Sum(_ => _.Price):#,##0.00}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }

        public static string GetDriveTrainSection(Dictionary<DriveTrain, List<Vehicle>> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty())
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportDetails.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{category_header_title}}", "Details By Drive Trains")
                    .Replace("{{category_item_list}}", GetDriveTrainContents(keyValuePairs));
            }

            return tableBody;
        }

        public static string GetDriveTrainContents(Dictionary<DriveTrain, List<Vehicle>> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if (pair.Value.IsNotNullOrEmpty())
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportItem.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{category_item_title}}", pair.Key.GetDescription())
                        .Replace("{{category_item_count}}", $"{pair.Value.Count:#,##}")
                        .Replace("{{category_item_total_price}}", $"{pair.Value.Sum(_ => _.Price):#,##0.00}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }

        public static string GetTransmissionSection(Dictionary<Transmission, List<Vehicle>> keyValuePairs)
        {
            string tableBody = string.Empty;
            if (keyValuePairs.IsNotNullOrEmpty())
            {
                var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportDetails.html");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (File.Exists(filepath))
                    tableBody = File.ReadAllText(filepath);
                else
                    return tableBody;

                tableBody = tableBody
                    .Replace("{{category_header_title}}", "Details By Transmission Types")
                    .Replace("{{category_item_list}}", GetTransmissionContents(keyValuePairs));
            }

            return tableBody;
        }

        public static string GetTransmissionContents(Dictionary<Transmission, List<Vehicle>> keyValuePairs)
        {
            var sb = new StringBuilder();
            var content = string.Empty;
            foreach (var pair in keyValuePairs)
            {
                if (pair.Value.IsNotNullOrEmpty())
                {
                    var folderName = Path.Combine("wwwroot", "PDF", "Subs", "CategorizedVehicleReportItem.html");
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (File.Exists(filepath))
                        content = File.ReadAllText(filepath);
                    else
                        return content;

                    sb.Append(content)
                        .Replace("{{category_item_title}}", pair.Key.GetDescription())
                        .Replace("{{category_item_count}}", $"{pair.Value.Count:#,##}")
                        .Replace("{{category_item_total_price}}", $"{pair.Value.Sum(_ => _.Price):#,##0.00}")
                        .AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
