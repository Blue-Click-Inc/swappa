using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace Swappa.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSpaceAndCapitalize(this string text)
        {
            if(text.Contains(' '))
            {
                var result = string.Empty;
                var words = text.Split(" ");

                foreach (var word in words)
                {
                    result += Capitalize(word);
                }

                return result;
            }
            else
            {
                return Capitalize(text);
            }
        }

        public static string Capitalize(this string text)
        {
            return Regex.Replace(text.ToLower().Trim(), "^[a-z]", m => m.Value.ToUpper());
        }

        public static string BuildUrl(this string origin, string token, TokenType tokenType = TokenType.AccountConfirmation)
        {
            var uriBuilder = new UriBuilder(origin!);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["Token"] = token;
            query["Type"] = tokenType.ToString();
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        public static string BuildBaseUrl(this StringValues origin) =>
            origin.IsNotNull() ? (new UriBuilder(origin!)).ToString() : string.Empty;

        public static string BuildAccountUrl(this StringValues origin, string token, TokenType tokenType)
        {
            var uriBuilder = new UriBuilder($"{origin}/accounts");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["Token"] = token;
            query["Type"] = tokenType.ToString();
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        public static bool IsNotNullOrEmpty(this string? value) =>
            !string.IsNullOrWhiteSpace(value);

        public static List<Claim>? ParseClaimsFromJwt(this string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePair = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePair?.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty)).ToList();
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
