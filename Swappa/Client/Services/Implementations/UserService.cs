using Microsoft.AspNetCore.Components.Authorization;
using Swappa.Client.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Swappa.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationState;
        private readonly HttpInterceptorService httpInterceptor;

        public UserService(HttpClient httpClient, 
            AuthenticationStateProvider authenticationState,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.authenticationState = authenticationState;
            this.httpInterceptor = httpInterceptor;
        }

        #region Users
        public async Task<Guid> GetLoggedInUserId()
        {
            var state = await authenticationState.GetAuthenticationStateAsync();
            var stringId = state?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            return stringId.ToGuid();
        }

        public async Task<ResponseModel<UserDetailsDto>?> GetUserByIdAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"user/{id}");

            return await httpInterceptor.Process<ResponseModel<UserDetailsDto>>(response); ;
        }

        public async Task<ResponseModel<string>?> UpdateDetailsAsync(Guid id, UserDetailsForUpdateDto request)
        {
            var response = await httpClient.PutAsJsonAsync<UserDetailsForUpdateDto>($"user/details/{id}", request);

            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<UserDashboardDataDto>?> GetUserDashboardAsync()
        {
            var response = await httpClient.GetAsync($"user/dashboard");

            return await httpInterceptor.Process<ResponseModel<UserDashboardDataDto>>(response);
        }

        public async Task<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>?> GetPagedUsersAsync(string request)
        {
            var response = await httpClient.GetAsync($"user{request}");

            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<LeanUserDetailsDto>>>(response);
        }

        #endregion

        #region Feedbacks
        public async Task<ResponseModel<string>?> SendFeedbackAsync(FeedbackForAddDto request)
        {
            var response = await httpClient.PostAsJsonAsync<FeedbackForAddDto>($"user/feedback/send", request);

            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> ToggleFeedbackAsync(Guid id)
        {
            var response = await httpClient.PatchAsync($"user/feedback/toggle/{id}", null);

            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> DeleteFeedbackAsync(Guid id)
        {
            var response = await httpClient.DeleteAsync($"user/feedback/{id}");

            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<PaginatedListDto<UserFeedbackDto>>?> GetUsersFeedbacks(string request)
        {
            var response = await httpClient.GetAsync($"user/feedback/all{request}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<UserFeedbackDto>>>(response);
        }

        public async Task<ResponseModel<FeedbackDashboardDto>?> FeedbackDashboardData()
        {
            var response = await httpClient.GetAsync($"user/feedback/dashboard");
            return await httpInterceptor.Process<ResponseModel<FeedbackDashboardDto>>(response);
        }
        #endregion
    }
}
