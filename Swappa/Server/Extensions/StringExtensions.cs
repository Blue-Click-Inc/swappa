using Microsoft.Extensions.Primitives;
using Swappa.Entities.Enums;
using System.Text.RegularExpressions;
using System.Web;

namespace Swappa.Server.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string text)
        {
            return Regex.Replace(text.ToLower().Trim(), "^[a-z]", m => m.Value.ToUpper());
        }

        public static string BuildUrl(this StringValues origin, string token, TokenType tokenType = TokenType.AccountConfirmation)
        {
            var uriBuilder = new UriBuilder(origin!);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["Token"] = token;
            query["Type"] = tokenType.ToString();
            //query["UserId"] = userId.ToString();
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
    }
}
