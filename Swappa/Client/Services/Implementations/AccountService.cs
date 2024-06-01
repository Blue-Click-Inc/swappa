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

        public async Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto request)
        {
            var response = await httpClient.PostAsJsonAsync("account/login", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<TokenDto>>();
        }

        public async Task<ResponseModel<string>?> RegisterAsync(RegisterDto request)
        {
            var response = await httpClient.PostAsJsonAsync("account/register", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        public async Task<ResponseModel<string>?> ConfirmAccountAsync(AccountConfirmationDto request)
        {
            var response = await httpClient.PutAsJsonAsync("account/confirm-email", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }

        public async Task<ResponseModel<string>?> ChangeForgotPasswordAsync(ForgotPasswordDto request)
        {
            var response = await httpClient.PutAsJsonAsync("account/forgot-password", request);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }
    }
}