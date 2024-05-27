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

        public async Task<ResponseModel<TokenDto>?> LoginAsync(LoginDto login)
        {
            var response = await httpClient.PostAsJsonAsync("account/login", login);

            return await response.Content.ReadFromJsonAsync<ResponseModel<TokenDto>>();
        }

        public async Task<ResponseModel<string>?> RegisterAsync(RegisterDto register)
        {
            var response = await httpClient.PostAsJsonAsync("account/register", register);

            return await response.Content.ReadFromJsonAsync<ResponseModel<string>>();
        }
    }
}