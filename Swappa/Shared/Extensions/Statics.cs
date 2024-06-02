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
    }
}
