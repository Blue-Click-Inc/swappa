using Swappa.Client.Services.Interfaces;
using Swappa.Shared.DTOs;
using System.Net.Http.Json;

namespace Swappa.Client.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient httpClient;
        private readonly HttpInterceptorService httpInterceptor;

        public LocationService(HttpClient httpClient,
            HttpInterceptorService httpInterceptor)
        {
            this.httpClient = httpClient;
            this.httpInterceptor = httpInterceptor;
        }

        public async Task<ResponseModel<CountryPaged>?> GetCountriesAsync()
        {
            var response = await httpClient.GetAsync($"location/countries");
            return await httpInterceptor.Process<ResponseModel<CountryPaged>>(response);
        }

        public async Task<ResponseModel<string>?> AddAsync(BaseLocationDto request)
        {
            var response = await httpClient.PostAsJsonAsync<BaseLocationDto>($"location", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<BaseLocationDto>?> GetAsync(Guid entityId)
        {
            var response = await httpClient.GetAsync($"location/{entityId}");
            return await httpInterceptor.Process<ResponseModel<BaseLocationDto>>(response);
        }

        public async Task<ResponseModel<string>?> UpdateAsync(BaseLocationDto request)
        {
            var response = await httpClient.PutAsJsonAsync<BaseLocationDto>($"location", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> DeleteAsync(Guid entityId)
        {
            var response = await httpClient.DeleteAsync($"location/{entityId}");
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }
    }
}
