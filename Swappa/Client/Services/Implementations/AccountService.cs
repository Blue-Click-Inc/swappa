using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Client side service for login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto request)
        {
            var response = await httpClient.PostAsJsonAsync("account/login", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<TokenDto>>();
        }

        /// <summary>
        /// Client side service for user registration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> RegisterAsync(RegisterDto request)
        {
            var response = await httpClient.PostAsJsonAsync("account/register", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        /// <summary>
        /// CLient side service for email account confirmation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> ConfirmAccountAsync(AccountConfirmationDto request)
        {
            var response = await httpClient.PutAsJsonAsync("account/confirm-email", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        /// <summary>
        /// Client side service for changing forgotten password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> ChangeForgotPasswordAsync(ForgotPasswordDto request)
        {
            var response = await httpClient.PutAsJsonAsync("account/forgot-password", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        /// <summary>
        /// Client side service for resetting password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> ResetPasswordAsync(EmailDto request)
        {
            var response = await httpClient.PutAsJsonAsync("account/reset-password", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        /// <summary>
        /// Client side service for changing user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> ChangePasswordAsync(Guid id, ChangePasswordDto request)
        {
            var response = await httpClient.PutAsJsonAsync($"account/change-password/{id}", request);
            
            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        /// <summary>
        /// Client side service for account deactivation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>?> DeactivateAccountAsync(Guid id)
        {
            var response = await httpClient.PutAsJsonAsync($"account/deactivate/{id}", new object());

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }
    }
}