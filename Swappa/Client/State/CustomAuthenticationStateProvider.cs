using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Swappa.Client.State
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal claimPrincipal = new(new ClaimsIdentity());
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient httpClient;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            this.httpClient = httpClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var accessToken = await localStorage.GetItemAsStringAsync("accessToken");
                var identity = new ClaimsIdentity();
                httpClient.DefaultRequestHeaders.Authorization = null;

                if (!string.IsNullOrEmpty(accessToken))
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken.Replace("\"", ""));
                }

                claimPrincipal = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(claimPrincipal);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            catch (Exception e)
            {
                return await Task.FromResult(new AuthenticationState(claimPrincipal));
            }
        }

        private static List<Claim>? ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePair = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePair?.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString())).ToList();
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

        public static string GetUserId(string jwt)
        {
            var userId = string.Empty;
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(jwt), "jwt");
            if (identity != null && identity.Claims.Any())
            {
                userId = identity.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            }
            
            return userId;
        }
    }
}
