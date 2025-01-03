using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public RoleService(HttpClient httpClient,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<PaginatedListDto<RoleDto>>?> GetAsync(int page = 1, int pageSize = 100)
        {
            var response = await httpClient.GetAsync($"role?PageNumber={page}&PageSize={pageSize}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<RoleDto>>>(response);
        }

        public async Task<ResponseModel<string>?> AddAsync(RoleForCreateDto request)
        {
            var response = await httpClient.PostAsJsonAsync($"role", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> UpdateAsync(Guid id, RoleForUpdateDto request)
        {
            var response = await httpClient.PutAsJsonAsync($"role/{id}", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> DeleteAsync(Guid id)
        {
            var response = await httpClient.DeleteAsync($"role/{id}");
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<RoleDto>?> GetAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"role/{id}");
            return await httpInterceptor.Process<ResponseModel<RoleDto>>(response);
        }
    }
}
