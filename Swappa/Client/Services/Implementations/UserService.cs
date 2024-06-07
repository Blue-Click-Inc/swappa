using Microsoft.AspNetCore.Components.Authorization;
using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Swappa.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationState;

        public UserService(HttpClient httpClient, AuthenticationStateProvider authenticationState)
        {
            this.httpClient = httpClient;
            this.authenticationState = authenticationState;
        }

        public async Task<ResponseModel<UserDetailsDto>?> GetUserByIdAsync(Guid id)
        {
            var response = await httpClient.GetFromJsonAsync<ResponseModel<UserDetailsDto>>($"user/{id}");

            return response;
        }

        public async Task<Guid> GetLoggedInUserId()
        {
            var state = await authenticationState.GetAuthenticationStateAsync();
            var stringId = state?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            return stringId.ToGuid();
        }
    }
}
