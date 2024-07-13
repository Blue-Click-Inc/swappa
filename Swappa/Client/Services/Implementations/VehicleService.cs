using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;

namespace Swappa.Client.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public VehicleService(HttpClient httpClient, HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query)
        {
            var response = await httpClient.GetAsync($"vehicle");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>(response);
        }
    }
}
