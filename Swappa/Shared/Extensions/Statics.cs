using Swappa.Entities.Models;

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
    }
}
