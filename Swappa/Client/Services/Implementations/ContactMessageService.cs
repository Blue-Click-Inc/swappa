using Amazon.Runtime.Internal;
using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public ContactMessageService(HttpClient httpClient,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>?> GetDataAsync(string query)
        {
            var response = await httpClient.GetAsync($"contact-message{query}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>>(response);
        }

        public async Task<ResponseModel<string>?> AddAsync(ContactMessageToAddDto request)
        {
            var response = await httpClient.PostAsJsonAsync($"contact-message", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<bool>?> ToggleIsRead(Guid id)
        {
            var response = await httpClient.PutAsJsonAsync($"contact-message/toggle-read/{id}", new object());
            return await httpInterceptor.Process<ResponseModel<bool>>(response);
        }

        public async Task<ResponseModel<ContactMessageToReturnDto>?> GetAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"contact-message/{id}");
            return await httpInterceptor.Process<ResponseModel<ContactMessageToReturnDto>>(response);
        }

        public async Task<ResponseModel<string>?> DeprecateManyAsync(List<Guid> ids)
        {
            var response = await httpClient.PutAsJsonAsync($"contact-message/deprecated-many", ids);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }
    }
}
