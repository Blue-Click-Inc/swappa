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

        public async Task<ResponseModel<string>?> UpdateDetailsAsync(Guid id, UserDetailsForUpdateDto request)
        {
            var response = await httpClient.PutAsJsonAsync<UserDetailsForUpdateDto>($"user/details/{id}", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        public async Task<ResponseModel<string>?> SendFeedbackAsync(FeedbackForAddDto request)
        {
            var response = await httpClient.PostAsJsonAsync<FeedbackForAddDto>($"user/feedback/send", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        public async Task<Guid> GetLoggedInUserId()
        {
            var state = await authenticationState.GetAuthenticationStateAsync();
            var stringId = state?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            return stringId.ToGuid();
        }
    }
}
