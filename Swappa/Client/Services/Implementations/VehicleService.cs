using BlazorBootstrap;
using Blazored.Modal;
using Swappa.Client.Pages.Modals.Vehicle;
using Swappa.Client.Services.Interfaces;
using Swappa.Entities.Enums;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;
using System.Net.Http.Json;

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

        public async Task<ResponseModel<VehicleToReturnDto>?> GetByIdAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"vehicle/{id}");
            return await httpInterceptor.Process<ResponseModel<VehicleToReturnDto>>(response);
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetDataAsync(VehicleQueryDto query)
        {
            var response = await httpClient.GetAsync($"vehicle{GetQuery(query)}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>(response);
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetFavoriteDataAsync(Guid userId, VehicleQueryDto query)
        {
            var response = await httpClient.GetAsync($"vehicle/{userId}/favorites{GetQuery(query)}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>(response);
        }

        public async Task<ResponseModel<PaginatedListDto<VehicleToReturnDto>>?> GetMerchantVehicleDataAsync(Guid merchantId, VehicleQueryDto query)
        {
            var response = await httpClient.GetAsync($"vehicle/{merchantId}/merchant{GetQuery(query)}");
            return await httpInterceptor.Process<ResponseModel<PaginatedListDto<VehicleToReturnDto>>>(response);
        }

        public async Task<ResponseModel<string>?> AddAsync(MultipartFormDataContent request)
        {
            var response = await httpClient.PostAsync($"vehicle", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<ResponseModel<string>?> UpdateAsync(Guid id, VehicleForUpdateDto request)
        {
            var response = await httpClient.PutAsJsonAsync($"vehicle/{id}", request);
            return await httpInterceptor.Process<ResponseModel<string>>(response);
        }

        public async Task<HttpResponseMessage?> ExportToExcel()
        {
            var response = await httpClient.GetAsync("vehicle/export-to-excel");
            return response;
        }

        public async Task<HttpResponseMessage?> PrintPDF()
        {
            var response = await httpClient.GetAsync("vehicle/print-pdf");
            return response;
        }

        public async Task<ResponseModel<VehicleDashboardDto>?> GetDashboard()
        {
            var response = await httpClient.GetAsync("vehicle/dashboard");
            return await httpInterceptor.Process<ResponseModel<VehicleDashboardDto>>(response);
        }

        public async Task<ResponseModel<FavoriteVehicleResponseDto>?> ToggleFavorite(IdDto request)
        {
            var response = await httpClient.PostAsJsonAsync("vehicle/toggle-favorite", request);
            return await httpInterceptor.Process<ResponseModel<FavoriteVehicleResponseDto>>(response);
        }

        public async Task<ResponseModel<bool>> IsFavorite(Guid id)
        {
            var response = await httpClient.GetAsync($"vehicle/is-favorite/{id}");
            if(response.IsNotNull() && response.IsSuccessStatusCode)
            {
                await response.Content.ReadFromJsonAsync<ResponseModel<bool>>();
            }
            return new ResponseModel<bool>();
        }

        public async Task<ResponseModel<long>?> GetFavoriteCount(Guid userId)
        {
            var response = await httpClient.GetAsync($"vehicle/favorite-count/{userId}");
            return await httpInterceptor.Process<ResponseModel<long>>(response);
        }

        private string GetQuery(VehicleQueryDto query)
        {
            var queryStr = string.Empty;
            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&SearchTerm={query.SearchTerm}" : $"?SearchTerm={query.SearchTerm}";
            }
            if(query.DriveTrain != DriveTrain.None)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&DriveTrain={query.DriveTrain}" : $"?DriveTrain={query.DriveTrain}";
            }
            if(query.Engine != Engine.None)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&Engine={query.Engine}" : $"?Engine={query.Engine}";
            }
            if (query.Transmission != Transmission.None)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&Transmission={query.Transmission}" : $"?Transmission={query.Transmission}";
            }
            if (query.PageSize != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageSize={query.PageSize}" : $"?PageSize={query.PageSize}";
            }
            if (query.PageNumber != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&PageNumber={query.PageNumber}" : $"?PageNumber={query.PageNumber}";
            }
            if (query.MinPrice != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&MinPrice={query.MinPrice}" : $"?MinPrice={query.MinPrice}";
            }
            if (query.MaxPrice != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&MaxPrice={query.MaxPrice}" : $"?MaxPrice={query.MaxPrice}";
            }
            if (query.MinYear != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&MinYear={query.MinYear}" : $"?MinYear={query.MinYear}";
            }
            if (query.MaxYear != default)
            {
                queryStr += queryStr.IsNotNullOrEmpty() ? $"&MaxYear={query.MaxYear}" : $"?MaxYear={query.MaxYear}";
            }
            return queryStr;
        }
    }
}
