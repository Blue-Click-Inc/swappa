using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Swappa.Shared.Extensions;
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

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, 
            HttpClient httpClient)
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
                    identity = new ClaimsIdentity(accessToken.ParseClaimsFromJwt(), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken.Replace("\"", ""));
                }

                claimPrincipal = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(claimPrincipal);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(claimPrincipal));
            }
        }
    }
}
