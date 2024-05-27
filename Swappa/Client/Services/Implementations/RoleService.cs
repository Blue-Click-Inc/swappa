using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient httpClient;

        public RoleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PaginatedListDto<RoleDto>?> GetRolesAsync(int page = 1, int pageSize = 10)
        {
            var response = await httpClient.GetFromJsonAsync<PaginatedListDto<RoleDto>>($"role?PageNumber={page}&PageSize={pageSize}");

            return response;
        }
    }
}
